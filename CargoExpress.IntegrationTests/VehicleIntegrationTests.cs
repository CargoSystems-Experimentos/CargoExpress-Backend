using System.Linq;
using Xunit;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;


namespace CargoExpress.IntegrationTests;

public class VehicleIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateVehicle_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var vehicleRepository = new VehicleRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var vehicle = new Vehicle
        {
            Model = "Volvo FH16",
            Plate = "ABC123",
            TractorPlate = "TRC456",
            MaxLoad = 20000f,
            Volume = 80f
        };

        await vehicleRepository.AddAsync(vehicle);
        await unitOfWork.CompleteAsync();

        var retrieved = await vehicleRepository.FindByIdAsync(vehicle.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Volvo FH16", retrieved.Model);
        Assert.Equal("ABC123", retrieved.Plate);
        Assert.Equal("TRC456", retrieved.TractorPlate);
        Assert.Equal(20000f, retrieved.MaxLoad);
        Assert.Equal(80f, retrieved.Volume);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllVehicles_ShouldReturnMultipleVehicles()
    {
        var dbContext = CreateDbContext();
        var vehicleRepository = new VehicleRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var vehicle1 = new Vehicle
        {
            Model = "Mercedes Actros",
            Plate = "XYZ001",
            TractorPlate = "TRC001",
            MaxLoad = 18000f,
            Volume = 70f
        };
        var vehicle2 = new Vehicle
        {
            Model = "Scania R500",
            Plate = "XYZ002",
            TractorPlate = "TRC002",
            MaxLoad = 22000f,
            Volume = 90f
        };

        await vehicleRepository.AddAsync(vehicle1);
        await vehicleRepository.AddAsync(vehicle2);
        await unitOfWork.CompleteAsync();

        var vehicles = await vehicleRepository.ListAsync();

        Assert.NotNull(vehicles);
        Assert.Equal(2, vehicles.Count());

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateVehicle_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var vehicleRepository = new VehicleRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var vehicle = new Vehicle
        {
            Model = "Old Model",
            Plate = "OLD001",
            TractorPlate = "TRC000",
            MaxLoad = 10000f,
            Volume = 50f
        };

        await vehicleRepository.AddAsync(vehicle);
        await unitOfWork.CompleteAsync();

        vehicle.Model = "New Model";
        vehicle.MaxLoad = 15000f;
        vehicleRepository.Update(vehicle);
        await unitOfWork.CompleteAsync();

        var updated = await vehicleRepository.FindByIdAsync(vehicle.Id);
        Assert.NotNull(updated);
        Assert.Equal("New Model", updated.Model);
        Assert.Equal(15000f, updated.MaxLoad);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetVehicleById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var vehicleRepository = new VehicleRepository(dbContext);

        var vehicle = await vehicleRepository.FindByIdAsync(999);

        Assert.Null(vehicle);

        CleanupDatabase(dbContext);
    }
}
