using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class VehicleIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateVehicle_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var vehicleRepository = new VehicleRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);
        var vehicle = new Vehicle("Volvo Truck", "Volvo FH16", "ABC123", "TRC456", 20000m, 80m, entrepreneur.Id, entrepreneur);

        await vehicleRepository.AddAsync(vehicle);
        await unitOfWork.CompleteAsync();

        var retrieved = await vehicleRepository.FindByIdAsync(vehicle.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Volvo Truck", retrieved.Name);
        Assert.Equal("Volvo FH16", retrieved.Model);
        Assert.Equal("ABC123", retrieved.Plate);
        Assert.Equal("TRC456", retrieved.TractorPlate);
        Assert.Equal(20000m, retrieved.MaxLoad);
        Assert.Equal(80m, retrieved.Volume);
        Assert.Equal("AVAILABLE", retrieved.State);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllVehicles_ShouldReturnMultipleVehicles()
    {
        var dbContext = CreateDbContext();
        var vehicleRepository = new VehicleRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);

        var vehicle1 = new Vehicle("Mercedes Truck", "Mercedes Actros", "XYZ001", "TRC001", 18000m, 70m, entrepreneur.Id, entrepreneur);
        var vehicle2 = new Vehicle("Scania Truck", "Scania R500", "XYZ002", "TRC002", 22000m, 90m, entrepreneur.Id, entrepreneur);

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

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);
        var vehicle = new Vehicle("Old Truck", "Old Model", "OLD001", "TRC000", 10000m, 50m, entrepreneur.Id, entrepreneur);

        await vehicleRepository.AddAsync(vehicle);
        await unitOfWork.CompleteAsync();

        vehicle.Name = "Updated Truck";
        vehicle.Model = "New Model";
        vehicle.MaxLoad = 15000m;
        vehicleRepository.Update(vehicle);
        await unitOfWork.CompleteAsync();

        var updated = await vehicleRepository.FindByIdAsync(vehicle.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Truck", updated.Name);
        Assert.Equal("New Model", updated.Model);
        Assert.Equal(15000m, updated.MaxLoad);

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

    [Fact]
    public async Task FindVehiclesByEntrepreneurId_ShouldReturnCorrectVehicles()
    {
        var dbContext = CreateDbContext();
        var vehicleRepository = new VehicleRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);
        var vehicle = new Vehicle("Volvo FH", "Volvo FH16 2022", "VLV001", "VLT001", 25000m, 100m, entrepreneur.Id, entrepreneur);

        await vehicleRepository.AddAsync(vehicle);
        await unitOfWork.CompleteAsync();

        var result = await vehicleRepository.FindByEntrepreneurIdAsync(entrepreneur.Id);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Volvo FH", result.First().Name);

        CleanupDatabase(dbContext);
    }
}
