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

        var driver = new Driver
        {
            Name = "Carlos Perez",
            Dni = "12345678",
            License = "A1B2C3D4E5",
            ContactNumber = "987654321"
        };

        await driverRepository.AddAsync(driver);
        await unitOfWork.CompleteAsync();

        var retrieved = await driverRepository.FindByIdAsync(driver.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("Carlos Perez", retrieved.Name);
        Assert.Equal("12345678", retrieved.Dni);
        Assert.Equal("A1B2C3D4E5", retrieved.License);
        Assert.Equal("987654321", retrieved.ContactNumber);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task GetAllDrivers_ShouldReturnMultipleDrivers()
    {
        var dbContext = CreateDbContext();
        var driverRepository = new DriverRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var driver1 = new Driver
        {
            Name = "Driver One",
            Dni = "11111111",
            License = "LIC0000001",
            ContactNumber = "111111111"
        };
        var driver2 = new Driver
        {
            Name = "Driver Two",
            Dni = "22222222",
            License = "LIC0000002",
            ContactNumber = "222222222"
        };

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

        var driver = new Driver
        {
            Name = "Original Name",
            Dni = "12345678",
            License = "ORIG123456",
            ContactNumber = "999999999"
        };

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
}
