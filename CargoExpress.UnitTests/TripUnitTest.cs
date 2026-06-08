using ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class TripUnitTest
{
    private readonly Mock<ITripRepository> _mockTripRepo;
    private readonly Mock<IDriverRepository> _mockDriverRepo;
    private readonly Mock<IVehicleRepository> _mockVehicleRepo;
    private readonly Mock<IClientRepository> _mockClientRepo;
    private readonly Mock<IEntrepreneurRepository> _mockEntrepreneurRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly TripCommandService _service;

    // Shared test doubles — IDs match what commands reference
    private static Client MakeClient() => new Client();
    private static Entrepreneur MakeEntrepreneur(int id = 1)
    {
        var e = new Entrepreneur();
        // Entrepreneur.Id defaults to 0 from parameterless ctor; set via reflection for the comparison
        typeof(Entrepreneur).BaseType?
            .GetProperty("Id")?
            .SetValue(e, id);
        return e;
    }

    private static Driver MakeDriver(int entrepreneurId = 1)
    {
        var d = new Driver();
        d.State = "AVAILABLE";
        // EntrepreneurId must match the entrepreneur's Id so ValidateDriverAsync passes
        typeof(Driver).GetProperty("EntrepreneurId")?.SetValue(d, entrepreneurId);
        return d;
    }

    private static Vehicle MakeVehicle(int entrepreneurId = 1)
    {
        var v = new Vehicle();
        v.State = "AVAILABLE";
        typeof(Vehicle).GetProperty("EntrepreneurId")?.SetValue(v, entrepreneurId);
        return v;
    }

    private static readonly DateTime LoadDate = new DateTime(2025, 8, 1, 8, 0, 0);
    private static readonly DateTime UnloadDate = new DateTime(2025, 8, 2, 8, 0, 0);

    public TripUnitTest()
    {
        _mockTripRepo = new Mock<ITripRepository>();
        _mockDriverRepo = new Mock<IDriverRepository>();
        _mockVehicleRepo = new Mock<IVehicleRepository>();
        _mockClientRepo = new Mock<IClientRepository>();
        _mockEntrepreneurRepo = new Mock<IEntrepreneurRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockClientRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(MakeClient());
        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(MakeEntrepreneur(0));
        _mockDriverRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(MakeDriver(0));
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(MakeVehicle(0));
        _mockTripRepo.Setup(r => r.FindByNameAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Trip?)null);
        _mockTripRepo.Setup(r => r.AddAsync(It.IsAny<Trip>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        _service = new TripCommandService(
            _mockTripRepo.Object,
            _mockDriverRepo.Object,
            _mockVehicleRepo.Object,
            _mockClientRepo.Object,
            _mockEntrepreneurRepo.Object,
            _mockUnitOfWork.Object);
    }

    // --- Entity construction ---

    [Fact]
    public void Create_Trip_WithParameterlessConstructor_DefaultStateIsAwaiting()
    {
        var trip = new Trip();
        Assert.Equal("AWAITING", trip.State);
    }

    [Fact]
    public void Create_Trip_WithAllParameters_SetsPropertiesCorrectly()
    {
        var driver = new Driver();
        var vehicle = new Vehicle();
        var client = new Client();
        var entrepreneur = new Entrepreneur();

        var trip = new Trip(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate,
            "Av. Peru 2", UnloadDate,
            1, 1, 1, 1,
            driver, vehicle, client, entrepreneur);

        Assert.Equal("Viaje Lima", trip.Name);
        Assert.Equal("Tecnologia", trip.Type);
        Assert.Equal(500m, trip.Weight);
        Assert.Equal("Av. Lima 1", trip.LoadLocation);
        Assert.Equal("Av. Peru 2", trip.UnloadLocation);
        Assert.Equal("AWAITING", trip.State);
    }

    // --- Repository mock tests ---

    [Fact]
    public async Task GetAll_Trips_Success()
    {
        var trips = new List<Trip> { new Trip(), new Trip() };
        _mockTripRepo.Setup(r => r.ListAsync()).ReturnsAsync(trips);

        var result = await _mockTripRepo.Object.ListAsync();

        _mockTripRepo.Verify(r => r.ListAsync(), Times.Once);
        Assert.Equal(trips, result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetById_Trip_ReturnsTrip_WhenExists()
    {
        var trip = new Trip();
        _mockTripRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(trip);
        _mockTripRepo.Setup(r => r.FindByIdAsync(0)).ReturnsAsync((Trip?)null);

        var found = await _mockTripRepo.Object.FindByIdAsync(1);
        var notFound = await _mockTripRepo.Object.FindByIdAsync(0);

        Assert.Equal(trip, found);
        Assert.Null(notFound);
    }

    // --- CreateTripCommand: valid scenario ---

    [Fact]
    public async Task Create_Trip_WithValidData_ReturnsTrip()
    {
        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate,
            "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var result = await _service.Handle(command);

        Assert.NotNull(result);
    }

    // --- Name validation ---

    [Fact]
    public async Task Create_Trip_WithEmptyName_ThrowsArgumentException()
    {
        var command = new CreateTripCommand(
            "", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("nombre", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithNameTooLong_ThrowsArgumentException()
    {
        var longName = new string('A', 61);
        var command = new CreateTripCommand(
            longName, "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("nombre", ex.Message);
    }

    // --- Type validation ---

    [Fact]
    public async Task Create_Trip_WithEmptyType_ThrowsArgumentException()
    {
        var command = new CreateTripCommand(
            "Viaje Lima", "", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("tipo", ex.Message);
    }

    // --- Weight validation ---

    [Fact]
    public async Task Create_Trip_WithZeroWeight_ThrowsArgumentException()
    {
        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 0m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("peso", ex.Message);
    }

    // --- Location validation ---

    [Fact]
    public async Task Create_Trip_WithEmptyLoadLocation_ThrowsArgumentException()
    {
        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("carga", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithEmptyUnloadLocation_ThrowsArgumentException()
    {
        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("descarga", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithSameLoadAndUnloadLocation_ThrowsArgumentException()
    {
        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Lima 1", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("igual", ex.Message);
    }

    // --- Date validation ---

    [Fact]
    public async Task Create_Trip_WithLoadDateAfterUnloadDate_ThrowsArgumentException()
    {
        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", UnloadDate, "Av. Peru 2", LoadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("fecha", ex.Message);
    }

    // --- Related entity not found ---

    [Fact]
    public async Task Create_Trip_WithNonExistentClientId_ThrowsArgumentException()
    {
        _mockClientRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Client?)null);

        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 99, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("cliente", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithNonExistentEntrepreneurId_ThrowsArgumentException()
    {
        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Entrepreneur?)null);

        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 99);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("empresario", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithNonExistentDriverId_ThrowsArgumentException()
    {
        _mockDriverRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Driver?)null);

        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            99, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("conductor", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithUnavailableDriver_ThrowsArgumentException()
    {
        var unavailableDriver = MakeDriver(0);
        unavailableDriver.State = "UNAVAILABLE";
        _mockDriverRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(unavailableDriver);

        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("conductor", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithNonExistentVehicleId_ThrowsArgumentException()
    {
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Vehicle?)null);

        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 99, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("vehículo", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithUnavailableVehicle_ThrowsArgumentException()
    {
        var unavailableVehicle = MakeVehicle(0);
        unavailableVehicle.State = "UNAVAILABLE";
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(unavailableVehicle);

        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("vehículo", ex.Message);
    }

    [Fact]
    public async Task Create_Trip_WithDuplicateTripName_ThrowsArgumentException()
    {
        _mockTripRepo.Setup(r => r.FindByNameAsync("Viaje Lima", It.IsAny<int>()))
            .ReturnsAsync(new Trip());

        var command = new CreateTripCommand(
            "Viaje Lima", "Tecnologia", 500m,
            "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate,
            1, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("registrado", ex.Message);
    }

    // --- UpdateTripStateCommand ---

    [Fact]
    public async Task Update_TripState_WithValidState_ReturnsTrip()
    {
        var trip = new Trip();
        _mockTripRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(trip);

        var command = new UpdateTripStateCommand(1, "PROGRESS");
        var result = await _service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("PROGRESS", result!.State);
    }

    [Fact]
    public async Task Update_TripState_WithInvalidState_ThrowsArgumentException()
    {
        var trip = new Trip();
        _mockTripRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(trip);

        var command = new UpdateTripStateCommand(1, "INVALID");

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("estado", ex.Message);
    }

    [Fact]
    public async Task Update_TripState_TripNotFound_ReturnsNull()
    {
        _mockTripRepo.Setup(r => r.FindByIdAsync(999)).ReturnsAsync((Trip?)null);

        var command = new UpdateTripStateCommand(999, "PROGRESS");
        var result = await _service.Handle(command);

        Assert.Null(result);
    }

    // --- UpdateTripDetailsCommand ---

    [Fact]
    public async Task Update_TripDetails_WithFinishedTrip_ThrowsInvalidOperationException()
    {
        var trip = new Trip { State = "FINISHED" };
        _mockTripRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(trip);

        var command = new UpdateTripDetailsCommand(1, "Viaje Nuevo", "Tipo", 100m, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Handle(command));
        Assert.Contains("finalizado", ex.Message);
    }

    [Fact]
    public async Task Update_TripDetails_WithCanceledTrip_ThrowsInvalidOperationException()
    {
        var trip = new Trip { State = "CANCELED" };
        _mockTripRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(trip);

        var command = new UpdateTripDetailsCommand(1, "Viaje Nuevo", "Tipo", 100m, 1, 1, 1);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Handle(command));
        Assert.Contains("cancelado", ex.Message);
    }

    // --- UpdateTripScheduleCommand ---

    [Fact]
    public async Task Update_TripSchedule_WithFinishedTrip_ThrowsInvalidOperationException()
    {
        var trip = new Trip { State = "FINISHED" };
        _mockTripRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(trip);

        var command = new UpdateTripScheduleCommand(1, "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Handle(command));
        Assert.Contains("finalizado", ex.Message);
    }

    [Fact]
    public async Task Update_TripSchedule_WithCanceledTrip_ThrowsInvalidOperationException()
    {
        var trip = new Trip { State = "CANCELED" };
        _mockTripRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(trip);

        var command = new UpdateTripScheduleCommand(1, "Av. Lima 1", LoadDate, "Av. Peru 2", UnloadDate);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.Handle(command));
        Assert.Contains("cancelado", ex.Message);
    }
}
