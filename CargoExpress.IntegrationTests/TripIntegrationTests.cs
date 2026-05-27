using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class TripIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateTrip_WithValidData_ShouldSucceed()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = new Trip
        {
            Name = "Lima to Callao",
            Type = "Electronics",
            Weight = 100,
            LoadLocation = "Av. Lima 123",
            LoadDate = DateTime.Now,
            UnloadLocation = "Av. Peru 456",
            UnloadDate = DateTime.Now.AddHours(3),
            DriverId = 1,
            VehicleId = 1,
            ClientId = 1,
            EntrepreneurId = 1
        };

        // Act
        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        // Assert
        var retrievedTrip = await tripRepository.FindByIdAsync(trip.Id);
        Assert.NotNull(retrievedTrip);
        Assert.Equal("Lima to Callao", retrievedTrip.Name);
        Assert.Equal("Electronics", retrievedTrip.Type);
        Assert.Equal(100, retrievedTrip.Weight);
        
        // Cleanup
        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllTrips_ShouldReturnMultipleTrips()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip1 = new Trip
        {
            Name = "Trip 1",
            Type = "Electronics",
            Weight = 100,
            LoadLocation = "Av. Lima 123",
            LoadDate = DateTime.Now,
            UnloadLocation = "Av. Peru 456",
            UnloadDate = DateTime.Now.AddHours(3),
            DriverId = 1,
            VehicleId = 1,
            ClientId = 1,
            EntrepreneurId = 1
        };
        
        var trip2 = new Trip
        {
            Name = "Trip 2",
            Type = "Food",
            Weight = 200,
            LoadLocation = "Av. San Borja 789",
            LoadDate = DateTime.Now,
            UnloadLocation = "Av. Vicus 321",
            UnloadDate = DateTime.Now.AddHours(4),
            DriverId = 1,
            VehicleId = 1,
            ClientId = 1,
            EntrepreneurId = 1
        };

        await tripRepository.AddAsync(trip1);
        await tripRepository.AddAsync(trip2);
        await unitOfWork.CompleteAsync();

        // Act
        var trips = await tripRepository.ListAsync();

        // Assert
        Assert.NotNull(trips);
        Assert.Equal(2, trips.Count());
        
        // Cleanup
        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateTrip_ShouldSucceed()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = new Trip
        {
            Name = "Original Description",
            Type = "Electronics",
            Weight = 100,
            LoadLocation = "Av. Lima 123",
            LoadDate = DateTime.Now,
            UnloadLocation = "Av. Peru 456",
            UnloadDate = DateTime.Now.AddHours(3),
            DriverId = 1,
            VehicleId = 1,
            ClientId = 1,
            EntrepreneurId = 1
        };
        
        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        // Act - Update trip
        trip.Name = "Updated Description";
        trip.Type = "Food";
        trip.Weight = 150;

        tripRepository.Update(trip);
        await unitOfWork.CompleteAsync();

        // Assert
        var updatedTrip = await tripRepository.FindByIdAsync(trip.Id);
        Assert.NotNull(updatedTrip);
        Assert.Equal("Updated Description", updatedTrip.Name);
        Assert.Equal("Food", updatedTrip.Type);
        Assert.Equal(150, updatedTrip.Weight);
        
        // Cleanup
        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetTripById_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);

        // Act
        var trip = await tripRepository.FindByIdAsync(999);

        // Assert
        Assert.Null(trip);
        
        // Cleanup
        CleanupDatabase(dbContext);
    }
}
