using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class AlertIntegrationTests : IntegrationTestBase
{
    private async Task<(ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration.AppDbContext dbContext,
        ACME.CargoExpress.API.Registration.Domain.Model.Aggregates.Trip trip)> SetupTripAsync(string tripName = "Alert Trip")
    {
        var (dbContext, driver, vehicle, client, entrepreneur) = await CreateTripSetupAsync();
        var trip = await CreateTripAsync(dbContext, tripName, driver, vehicle, client, entrepreneur);
        return (dbContext, trip);
    }

    [Fact]
    public async Task CreateAlert_WithValidData_ShouldSucceed()
    {
        var (dbContext, trip) = await SetupTripAsync();
        var alertRepository = new AlertRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var command = new CreateAlertCommand("Demora en ruta", "WARNING", "El conductor reporta trafico en la via", DateTime.Now, trip.Id);
        var alert = new Alert(command, trip);

        await alertRepository.AddAsync(alert);
        await unitOfWork.CompleteAsync();

        var retrieved = await alertRepository.FindByIdAsync(alert.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Demora en ruta", retrieved.Title);
        Assert.Equal("WARNING", retrieved.Type);
        Assert.Equal("El conductor reporta trafico en la via", retrieved.Description);
        Assert.Equal(trip.Id, retrieved.TripId);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindAlertsByTripId_ShouldReturnAlerts()
    {
        var (dbContext, trip) = await SetupTripAsync();
        var alertRepository = new AlertRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var cmd1 = new CreateAlertCommand("Alerta 1", "WARNING", "Descripcion 1", DateTime.Now, trip.Id);
        var cmd2 = new CreateAlertCommand("Alerta 2", "DANGER", "Descripcion 2", DateTime.Now.AddMinutes(5), trip.Id);

        await alertRepository.AddAsync(new Alert(cmd1, trip));
        await alertRepository.AddAsync(new Alert(cmd2, trip));
        await unitOfWork.CompleteAsync();

        var alerts = await alertRepository.FindByTripIdAsync(trip.Id);

        Assert.NotNull(alerts);
        Assert.Equal(2, alerts.Count());

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateAlert_ShouldSucceed()
    {
        var (dbContext, trip) = await SetupTripAsync();
        var alertRepository = new AlertRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var createCmd = new CreateAlertCommand("Original Title", "INFO", "Original Description", DateTime.Now, trip.Id);
        var alert = new Alert(createCmd, trip);

        await alertRepository.AddAsync(alert);
        await unitOfWork.CompleteAsync();

        var updateCmd = new UpdateAlertCommand(alert.Id, "Updated Title", "WARNING", "Updated Description", DateTime.Now);
        alert.Update(updateCmd);
        alertRepository.Update(alert);
        await unitOfWork.CompleteAsync();

        var updated = await alertRepository.FindByIdAsync(alert.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Title", updated.Title);
        Assert.Equal("WARNING", updated.Type);
        Assert.Equal("Updated Description", updated.Description);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindAlertsByTripId_WithNoAlerts_ShouldReturnEmpty()
    {
        var dbContext = CreateDbContext();
        var alertRepository = new AlertRepository(dbContext);

        var alerts = await alertRepository.FindByTripIdAsync(999);

        Assert.NotNull(alerts);
        Assert.Empty(alerts);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAlertById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var alertRepository = new AlertRepository(dbContext);

        var alert = await alertRepository.FindByIdAsync(999);

        Assert.Null(alert);

        CleanupDatabase(dbContext);
    }
}
