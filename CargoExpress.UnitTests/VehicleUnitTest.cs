using ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;
using ACME.CargoExpress.API.Registration.Domain.Exceptions;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class VehicleUnitTest
{
    private readonly Mock<IVehicleRepository> _mockVehicleRepo;
    private readonly Mock<IEntrepreneurRepository> _mockEntrepreneurRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly VehicleCommandService _service;

    private static readonly Entrepreneur ValidEntrepreneur = new Entrepreneur();

    public VehicleUnitTest()
    {
        _mockVehicleRepo = new Mock<IVehicleRepository>();
        _mockEntrepreneurRepo = new Mock<IEntrepreneurRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(ValidEntrepreneur);
        _mockVehicleRepo.Setup(r => r.FindByNameAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Vehicle?)null);
        _mockVehicleRepo.Setup(r => r.FindByPlateAsync(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync((Vehicle?)null);
        _mockVehicleRepo.Setup(r => r.AddAsync(It.IsAny<Vehicle>())).Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        _service = new VehicleCommandService(
            _mockVehicleRepo.Object,
            _mockEntrepreneurRepo.Object,
            _mockUnitOfWork.Object);
    }

    // --- Entity construction ---

    [Fact]
    public void Create_Vehicle_WithParameterlessConstructor_DefaultStateIsAvailable()
    {
        var vehicle = new Vehicle();
        Assert.Equal("AVAILABLE", vehicle.State);
    }

    [Fact]
    public void Create_Vehicle_WithAllParameters_SetsPropertiesCorrectly()
    {
        var entrepreneur = new Entrepreneur();
        var vehicle = new Vehicle("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", 5000m, 30m, 1, entrepreneur);

        Assert.Equal("Camion Frio", vehicle.Name);
        Assert.Equal("Volvo FH", vehicle.Model);
        Assert.Equal("ABC-123", vehicle.Plate);
        Assert.Equal("DEF-456", vehicle.TractorPlate);
        Assert.Equal(5000m, vehicle.MaxLoad);
        Assert.Equal(30m, vehicle.Volume);
        Assert.Equal(1, vehicle.EntrepreneurId);
        Assert.Equal("AVAILABLE", vehicle.State);
    }

    // --- Repository mock tests ---

    [Fact]
    public async Task GetAll_Vehicles_Success()
    {
        var vehicles = new List<Vehicle> { new Vehicle(), new Vehicle() };
        _mockVehicleRepo.Setup(r => r.ListAsync()).ReturnsAsync(vehicles);

        var result = await _mockVehicleRepo.Object.ListAsync();

        _mockVehicleRepo.Verify(r => r.ListAsync(), Times.Once);
        Assert.Equal(vehicles, result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetById_Vehicle_ReturnsVehicle_WhenExists()
    {
        var vehicle = new Vehicle();
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(vehicle);
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(0)).ReturnsAsync((Vehicle?)null);

        var found = await _mockVehicleRepo.Object.FindByIdAsync(1);
        var notFound = await _mockVehicleRepo.Object.FindByIdAsync(0);

        Assert.Equal(vehicle, found);
        Assert.Null(notFound);
    }

    [Fact]
    public async Task Add_Vehicle_Success()
    {
        var vehicle = new Vehicle();
        _mockVehicleRepo.Setup(r => r.AddAsync(vehicle)).Returns(Task.CompletedTask);

        await _mockVehicleRepo.Object.AddAsync(vehicle);

        _mockVehicleRepo.Verify(r => r.AddAsync(vehicle), Times.Once);
    }

    // --- Command service: valid creation ---

    [Fact]
    public async Task Create_Vehicle_WithValidData_ReturnsVehicle()
    {
        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", 5000m, 30m, 1);

        var result = await _service.Handle(command);

        Assert.NotNull(result);
    }

    // --- Name validation ---

    [Fact]
    public async Task Create_Vehicle_WithEmptyName_ThrowsInvalidVehicleNameException()
    {
        var command = new CreateVehicleCommand("", "Volvo FH", "ABC-123", "DEF-456", 5000m, 30m, 1);

        await Assert.ThrowsAsync<InvalidVehicleNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Vehicle_WithNameTooLong_ThrowsVehicleNameTooLongException()
    {
        var longName = new string('A', 61);
        var command = new CreateVehicleCommand(longName, "Volvo FH", "ABC-123", "DEF-456", 5000m, 30m, 1);

        await Assert.ThrowsAsync<VehicleNameTooLongException>(() => _service.Handle(command));
    }

    // --- Model validation ---

    [Fact]
    public async Task Create_Vehicle_WithEmptyModel_ThrowsInvalidVehicleModelException()
    {
        var command = new CreateVehicleCommand("Camion Frio", "", "ABC-123", "DEF-456", 5000m, 30m, 1);

        await Assert.ThrowsAsync<InvalidVehicleModelException>(() => _service.Handle(command));
    }

    // --- Plate validation ---

    [Fact]
    public async Task Create_Vehicle_WithEmptyPlate_ThrowsInvalidVehiclePlateException()
    {
        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "", "DEF-456", 5000m, 30m, 1);

        await Assert.ThrowsAsync<InvalidVehiclePlateException>(() => _service.Handle(command));
    }

    // --- TractorPlate validation ---

    [Fact]
    public async Task Create_Vehicle_WithEmptyTractorPlate_ThrowsInvalidVehicleTractorPlateException()
    {
        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "", 5000m, 30m, 1);

        await Assert.ThrowsAsync<InvalidVehicleTractorPlateException>(() => _service.Handle(command));
    }

    // --- MaxLoad validation ---

    [Fact]
    public async Task Create_Vehicle_WithZeroMaxLoad_ThrowsInvalidVehicleMaxLoadRangeException()
    {
        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", 0m, 30m, 1);

        await Assert.ThrowsAsync<InvalidVehicleMaxLoadRangeException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Vehicle_WithNegativeMaxLoad_ThrowsInvalidVehicleMaxLoadRangeException()
    {
        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", -1m, 30m, 1);

        await Assert.ThrowsAsync<InvalidVehicleMaxLoadRangeException>(() => _service.Handle(command));
    }

    // --- Volume validation ---

    [Fact]
    public async Task Create_Vehicle_WithZeroVolume_ThrowsInvalidVehicleVolumeRangeException()
    {
        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", 5000m, 0m, 1);

        await Assert.ThrowsAsync<InvalidVehicleVolumeRangeException>(() => _service.Handle(command));
    }

    // --- Entrepreneur not found ---

    [Fact]
    public async Task Create_Vehicle_WithNonExistentEntrepreneurId_ThrowsArgumentException()
    {
        _mockEntrepreneurRepo.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((Entrepreneur?)null);

        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", 5000m, 30m, 99);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => _service.Handle(command));
        Assert.Contains("empresario", ex.Message);
    }

    // --- Duplicate checks ---

    [Fact]
    public async Task Create_Vehicle_WithDuplicateName_ThrowsDuplicateVehicleNameException()
    {
        _mockVehicleRepo.Setup(r => r.FindByNameAsync("Camion Frio", It.IsAny<int>()))
            .ReturnsAsync(new Vehicle());

        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", 5000m, 30m, 1);

        await Assert.ThrowsAsync<DuplicateVehicleNameException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Create_Vehicle_WithDuplicatePlate_ThrowsDuplicateVehiclePlateException()
    {
        _mockVehicleRepo.Setup(r => r.FindByPlateAsync("ABC-123", It.IsAny<int>()))
            .ReturnsAsync(new Vehicle());

        var command = new CreateVehicleCommand("Camion Frio", "Volvo FH", "ABC-123", "DEF-456", 5000m, 30m, 1);

        await Assert.ThrowsAsync<DuplicateVehiclePlateException>(() => _service.Handle(command));
    }

    // --- UpdateVehicleStateCommand validation ---

    [Fact]
    public async Task Update_VehicleState_WithValidState_ReturnsVehicle()
    {
        var vehicle = new Vehicle();
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(vehicle);

        var command = new UpdateVehicleStateCommand(1, "UNAVAILABLE");
        var result = await _service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("UNAVAILABLE", result!.State);
    }

    [Fact]
    public async Task Update_VehicleState_WithInvalidState_ThrowsInvalidVehicleStateException()
    {
        var vehicle = new Vehicle();
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(vehicle);

        var command = new UpdateVehicleStateCommand(1, "BROKEN");

        await Assert.ThrowsAsync<InvalidVehicleStateException>(() => _service.Handle(command));
    }

    [Fact]
    public async Task Update_VehicleState_WithInactiveState_Succeeds()
    {
        var vehicle = new Vehicle();
        _mockVehicleRepo.Setup(r => r.FindByIdAsync(1)).ReturnsAsync(vehicle);

        var command = new UpdateVehicleStateCommand(1, "INACTIVE");
        var result = await _service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("INACTIVE", result!.State);
    }
}
