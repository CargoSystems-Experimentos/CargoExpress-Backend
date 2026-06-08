using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class TripIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateTrip_WithValidData_ShouldSucceed()
    {
        var (dbContext, driver, vehicle, client, entrepreneur) = await CreateTripSetupAsync();
        var tripRepository = new TripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = new Trip("Lima to Callao", "Electronics", 100m,
            "Av. Lima 123", DateTime.Now,
            "Av. Peru 456", DateTime.Now.AddHours(3),
            driver.Id, vehicle.Id, client.Id, entrepreneur.Id,
            driver, vehicle, client, entrepreneur);

        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        var retrievedTrip = await tripRepository.FindByIdAsync(trip.Id);
        Assert.NotNull(retrievedTrip);
        Assert.Equal("Lima to Callao", retrievedTrip.Name);
        Assert.Equal("Electronics", retrievedTrip.Type);
        Assert.Equal(100m, retrievedTrip.Weight);
        Assert.Equal("AWAITING", retrievedTrip.State);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllTrips_ShouldReturnMultipleTrips()
    {
        var (dbContext, driver, vehicle, client, entrepreneur) = await CreateTripSetupAsync();
        var tripRepository = new TripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip1 = new Trip("Trip 1", "Electronics", 100m,
            "Av. Lima 123", DateTime.Now,
            "Av. Peru 456", DateTime.Now.AddHours(3),
            driver.Id, vehicle.Id, client.Id, entrepreneur.Id,
            driver, vehicle, client, entrepreneur);

        var trip2 = new Trip("Trip 2", "Food", 200m,
            "Av. San Borja 789", DateTime.Now,
            "Av. Vicus 321", DateTime.Now.AddHours(4),
            driver.Id, vehicle.Id, client.Id, entrepreneur.Id,
            driver, vehicle, client, entrepreneur);

        await tripRepository.AddAsync(trip1);
        await tripRepository.AddAsync(trip2);
        await unitOfWork.CompleteAsync();

        var trips = await tripRepository.ListAsync();

        Assert.NotNull(trips);
        Assert.Equal(2, trips.Count());

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateTrip_ShouldSucceed()
    {
        var (dbContext, driver, vehicle, client, entrepreneur) = await CreateTripSetupAsync();
        var tripRepository = new TripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = new Trip("Original Name", "Electronics", 100m,
            "Av. Lima 123", DateTime.Now,
            "Av. Peru 456", DateTime.Now.AddHours(3),
            driver.Id, vehicle.Id, client.Id, entrepreneur.Id,
            driver, vehicle, client, entrepreneur);

        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        trip.Name = "Updated Name";
        trip.Type = "Food";
        trip.Weight = 150m;

        tripRepository.Update(trip);
        await unitOfWork.CompleteAsync();

        var updatedTrip = await tripRepository.FindByIdAsync(trip.Id);
        Assert.NotNull(updatedTrip);
        Assert.Equal("Updated Name", updatedTrip.Name);
        Assert.Equal("Food", updatedTrip.Type);
        Assert.Equal(150m, updatedTrip.Weight);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetTripById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);

        var trip = await tripRepository.FindByIdAsync(999);

        Assert.Null(trip);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindTripsByClientId_ShouldReturnCorrectTrips()
    {
        var (dbContext, driver, vehicle, client, entrepreneur) = await CreateTripSetupAsync();
        var tripRepository = new TripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = new Trip("Client Trip", "Electronics", 100m,
            "Av. Lima 123", DateTime.Now,
            "Av. Peru 456", DateTime.Now.AddHours(3),
            driver.Id, vehicle.Id, client.Id, entrepreneur.Id,
            driver, vehicle, client, entrepreneur);

        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        var trips = await tripRepository.FindByClientIdAsync(client.Id);

        Assert.NotNull(trips);
        Assert.Single(trips);
        Assert.Equal("Client Trip", trips.First().Name);

        CleanupDatabase(dbContext);
    }
}
