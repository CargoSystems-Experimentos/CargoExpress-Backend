using ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class AlertUnitTest
{
    // ---------------------------------------------------------------
    // Existing repository-mock tests (preserved)
    // ---------------------------------------------------------------

    [Fact]
    public async Task GetAll_Alerts_Success()
    {
        // Arrange
        var alerts = new List<Alert> { new Alert(), new Alert() };
        var mockAlertRepository = new Mock<IAlertRepository>();
        mockAlertRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(alerts);

        // Act
        var returnedAlerts = await mockAlertRepository.Object.ListAsync();

        // Assert
        mockAlertRepository.Verify(repo => repo.ListAsync(), Times.Once);
        Assert.Equal(alerts, returnedAlerts);
        Assert.Equal(alerts.Count, returnedAlerts.Count());
    }

    [Fact]
    public async Task GetById_Alert_Success()
    {
        // Arrange
        int validId = 1;
        var alert = new Alert();
        var mockAlertRepository = new Mock<IAlertRepository>();
        mockAlertRepository.Setup(repo => repo.FindByIdAsync(validId)).ReturnsAsync(alert);

        // Act
        var returnedAlert = await mockAlertRepository.Object.FindByIdAsync(validId);

        // Assert
        mockAlertRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
        Assert.Equal(alert, returnedAlert);
    }

    [Fact]
    public async Task Add_Alert_Success()
    {
        // Arrange
        var alert = new Alert();
        var mockAlertRepository = new Mock<IAlertRepository>();
        mockAlertRepository.Setup(repo => repo.AddAsync(alert)).Returns(Task.CompletedTask);

        // Act
        await mockAlertRepository.Object.AddAsync(alert);

        // Assert
        mockAlertRepository.Verify(repo => repo.AddAsync(alert), Times.Once);
    }

    [Fact]
    public void Update_Alert_Success()
    {
        // Arrange
        var alert = new Alert();
        var command = new UpdateAlertCommand(1, "Alerta actualizada", "WARNING", "Descripción nueva", new DateTime(2024, 8, 1));

        // Act
        alert.Update(command);

        // Assert
        Assert.Equal("Alerta actualizada", alert.Title);
        Assert.Equal("WARNING", alert.Type);
        Assert.Equal("Descripción nueva", alert.Description);
        Assert.Equal(new DateTime(2024, 8, 1), alert.Date);
    }

    // ---------------------------------------------------------------
    // AlertCommandService tests
    // ---------------------------------------------------------------

    private static readonly DateTime LoadDate = new DateTime(2025, 8, 1, 8, 0, 0);
    private static readonly DateTime UnloadDate = new DateTime(2025, 8, 3, 8, 0, 0);
    private static readonly DateTime ValidAlertDate = new DateTime(2025, 8, 2, 8, 0, 0);

    private static Trip MakeProgressTrip()
    {
        var trip = new Trip
        {
            State = "PROGRESS",
            LoadDate = LoadDate,
            UnloadDate = UnloadDate
        };
        return trip;
    }

    private (AlertCommandService service, Mock<IAlertRepository> alertRepo, Mock<ITripRepository> tripRepo)
        BuildService(Trip? tripForId = null)
    {
        var mockAlertRepo = new Mock<IAlertRepository>();
        var mockTripRepo = new Mock<ITripRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockTripRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>()))
            .ReturnsAsync(tripForId ?? MakeProgressTrip());
        mockAlertRepo.Setup(r => r.AddAsync(It.IsAny<Alert>())).Returns(Task.CompletedTask);
        mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var service = new AlertCommandService(
            mockAlertRepo.Object,
            mockTripRepo.Object,
            mockUnitOfWork.Object);

        return (service, mockAlertRepo, mockTripRepo);
    }

    // --- Valid creation ---

    [Fact]
    public async Task Create_Alert_WithValidData_ReturnsAlert()
    {
        var (service, _, _) = BuildService();
        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", "Hay un desvío en ruta", ValidAlertDate, 1);

        var result = await service.Handle(command);

        Assert.NotNull(result);
        Assert.Equal("Alerta de tráfico", result!.Title);
    }

    // --- Title validation ---

    [Fact]
    public async Task Create_Alert_WithEmptyTitle_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateAlertCommand("", "WARNING", "Descripción válida de la alerta", ValidAlertDate, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("título", ex.Message);
    }

    [Fact]
    public async Task Create_Alert_WithTitleTooLong_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var longTitle = new string('A', 61);
        var command = new CreateAlertCommand(longTitle, "WARNING", "Descripción válida", ValidAlertDate, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("título", ex.Message);
    }

    // --- Type validation ---

    [Fact]
    public async Task Create_Alert_WithEmptyType_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateAlertCommand("Alerta de tráfico", "", "Descripción válida", ValidAlertDate, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("tipo", ex.Message);
    }

    // --- Description validation ---

    [Fact]
    public async Task Create_Alert_WithEmptyDescription_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", "", ValidAlertDate, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("descripción", ex.Message);
    }

    [Fact]
    public async Task Create_Alert_WithDescriptionTooLong_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var longDesc = new string('A', 101);
        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", longDesc, ValidAlertDate, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("descripción", ex.Message);
    }

    // --- Trip not found ---

    [Fact]
    public async Task Create_Alert_WithNonExistentTripId_ThrowsArgumentException()
    {
        var mockAlertRepo = new Mock<IAlertRepository>();
        var mockTripRepo = new Mock<ITripRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        mockTripRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Trip?)null);
        var service = new AlertCommandService(mockAlertRepo.Object, mockTripRepo.Object, mockUnitOfWork.Object);

        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", "Descripción válida", ValidAlertDate, 99);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("viaje", ex.Message);
    }

    // --- Trip not in PROGRESS ---

    [Fact]
    public async Task Create_Alert_WhenTripNotInProgress_ThrowsInvalidOperationException()
    {
        var awaitingTrip = new Trip { State = "AWAITING", LoadDate = LoadDate, UnloadDate = UnloadDate };
        var (service, _, _) = BuildService(awaitingTrip);

        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", "Descripción válida", ValidAlertDate, 1);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Handle(command));
        Assert.Contains("progreso", ex.Message);
    }

    // --- Date validation ---

    [Fact]
    public async Task Create_Alert_WithDateEqualToLoadDate_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", "Descripción válida", LoadDate, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("fecha", ex.Message);
    }

    [Fact]
    public async Task Create_Alert_WithDateEqualToUnloadDate_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", "Descripción válida", UnloadDate, 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("fecha", ex.Message);
    }

    [Fact]
    public async Task Create_Alert_WithDateBeforeLoadDate_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateAlertCommand("Alerta de tráfico", "WARNING", "Descripción válida", LoadDate.AddHours(-1), 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("fecha", ex.Message);
    }
}
