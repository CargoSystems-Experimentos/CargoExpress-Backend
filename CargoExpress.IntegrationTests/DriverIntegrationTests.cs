using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class DriverIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateDriver_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var driverRepository = new DriverRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);
        var driver = new Driver("Carlos Perez", "12345678", "A1B2C3D4E5", "987654321", entrepreneur.Id, entrepreneur);

        await driverRepository.AddAsync(driver);
        await unitOfWork.CompleteAsync();

        var retrieved = await driverRepository.FindByIdAsync(driver.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Carlos Perez", retrieved.Name);
        Assert.Equal("12345678", retrieved.Dni);
        Assert.Equal("A1B2C3D4E5", retrieved.License);
        Assert.Equal("987654321", retrieved.ContactNumber);
        Assert.Equal("AVAILABLE", retrieved.State);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllDrivers_ShouldReturnMultipleDrivers()
    {
        var dbContext = CreateDbContext();
        var driverRepository = new DriverRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);

        var driver1 = new Driver("Driver One", "11111111", "LIC0000001", "111111111", entrepreneur.Id, entrepreneur);
        var driver2 = new Driver("Driver Two", "22222222", "LIC0000002", "222222222", entrepreneur.Id, entrepreneur);

        await driverRepository.AddAsync(driver1);
        await driverRepository.AddAsync(driver2);
        await unitOfWork.CompleteAsync();

        var drivers = await driverRepository.ListAsync();

        Assert.NotNull(drivers);
        Assert.Equal(2, drivers.Count());

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateDriver_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var driverRepository = new DriverRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);
        var driver = new Driver("Original Name", "12345678", "ORIG123456", "999999999", entrepreneur.Id, entrepreneur);

        await driverRepository.AddAsync(driver);
        await unitOfWork.CompleteAsync();

        driver.Name = "Updated Name";
        driver.License = "UPDT123456";
        driverRepository.Update(driver);
        await unitOfWork.CompleteAsync();

        var updated = await driverRepository.FindByIdAsync(driver.Id);
        Assert.NotNull(updated);
        Assert.Equal("Updated Name", updated.Name);
        Assert.Equal("UPDT123456", updated.License);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetDriverById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var driverRepository = new DriverRepository(dbContext);

        var driver = await driverRepository.FindByIdAsync(999);

        Assert.Null(driver);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindDriversByEntrepreneurId_ShouldReturnCorrectDrivers()
    {
        var dbContext = CreateDbContext();
        var driverRepository = new DriverRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);
        var driver = new Driver("Carlos Lopez", "11111111", "LIC0000001", "111111111", entrepreneur.Id, entrepreneur);

        await driverRepository.AddAsync(driver);
        await unitOfWork.CompleteAsync();

        var result = await driverRepository.FindByEntrepreneurIdAsync(entrepreneur.Id);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Carlos Lopez", result.First().Name);

        CleanupDatabase(dbContext);
    }
}
