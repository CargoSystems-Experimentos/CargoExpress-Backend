using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Infrastructure;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class OngoingTripIntegrationTests : IntegrationTestBase
{
    private async Task<(ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration.AppDbContext dbContext,
        ACME.CargoExpress.API.Registration.Domain.Model.Aggregates.Trip trip)> SetupTripAsync()
    {
        var (dbContext, driver, vehicle, client, entrepreneur) = await CreateTripSetupAsync();
        var trip = await CreateTripAsync(dbContext, "Ongoing Trip Test", driver, vehicle, client, entrepreneur);
        return (dbContext, trip);
    }

    [Fact]
    public async Task CreateOngoingTrip_WithValidData_ShouldSucceed()
    {
        var (dbContext, trip) = await SetupTripAsync();
        var ongoingTripRepository = new OngoingTripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var ongoingTrip = new OngoingTrip(-12.046374f, -77.042793f, 60, 15, trip.Id, trip);

        await ongoingTripRepository.AddAsync(ongoingTrip);
        await unitOfWork.CompleteAsync();

        var retrieved = await ongoingTripRepository.FindByIdAsync(ongoingTrip.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(-12.046374f, retrieved.Latitude, 5);
        Assert.Equal(-77.042793f, retrieved.Longitude, 5);
        Assert.Equal(60, retrieved.Speed);
        Assert.Equal(15, retrieved.Distance);
        Assert.Equal(trip.Id, retrieved.TripId);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindOngoingTripByTripId_ShouldReturnOngoingTrip()
    {
        var (dbContext, trip) = await SetupTripAsync();
        var ongoingTripRepository = new OngoingTripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var ongoingTrip = new OngoingTrip(-12.046374f, -77.042793f, 80, 30, trip.Id, trip);

        await ongoingTripRepository.AddAsync(ongoingTrip);
        await unitOfWork.CompleteAsync();

        var found = await ongoingTripRepository.FindByTripIdAsync(trip.Id);

        Assert.NotNull(found);
        Assert.Equal(trip.Id, found.TripId);
        Assert.Equal(80, found.Speed);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateOngoingTrip_ShouldSucceed()
    {
        var (dbContext, trip) = await SetupTripAsync();
        var ongoingTripRepository = new OngoingTripRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var ongoingTrip = new OngoingTrip(-12.046374f, -77.042793f, 60, 10, trip.Id, trip);

        await ongoingTripRepository.AddAsync(ongoingTrip);
        await unitOfWork.CompleteAsync();

        ongoingTrip.Speed = 90;
        ongoingTrip.Distance = 50;
        ongoingTripRepository.Update(ongoingTrip);
        await unitOfWork.CompleteAsync();

        var updated = await ongoingTripRepository.FindByIdAsync(ongoingTrip.Id);
        Assert.NotNull(updated);
        Assert.Equal(90, updated.Speed);
        Assert.Equal(50, updated.Distance);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindOngoingTripByTripId_WithNoData_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var ongoingTripRepository = new OngoingTripRepository(dbContext);

        var result = await ongoingTripRepository.FindByTripIdAsync(999);

        Assert.Null(result);

        CleanupDatabase(dbContext);
    }
}
