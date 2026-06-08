using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using IAMUser = ACME.CargoExpress.API.IAM.Domain.Model.Aggregates.User;

namespace CargoExpress.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        var dbContext = new AppDbContext(options);
        dbContext.Database.OpenConnection();
        dbContext.Database.EnsureCreated();

        return dbContext;
    }

    protected void CleanupDatabase(AppDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }

    protected async Task<IAMUser> CreateUserAsync(AppDbContext ctx, string username, string phone, bool isEntrepreneur = false)
    {
        var user = new IAMUser(username, "hashedpassword", phone, isEntrepreneur);
        await ctx.Users.AddAsync(user);
        await ctx.SaveChangesAsync();
        return user;
    }

    protected async Task<Entrepreneur> CreateEntrepreneurAsync(AppDbContext ctx, IAMUser user,
        string name = "Test Company SAC", string ruc = "20123456789", string address = "Av. Test 123")
    {
        var entrepreneur = new Entrepreneur(name, ruc, address, user.Id, user);
        await ctx.Entrepreneurs.AddAsync(entrepreneur);
        await ctx.SaveChangesAsync();
        return entrepreneur;
    }

    protected async Task<Client> CreateClientAsync(AppDbContext ctx, IAMUser user,
        string name = "Test Client", string dni = "12345678")
    {
        var client = new Client(name, dni, new DateTime(1990, 1, 1), user.Id, user);
        await ctx.Clients.AddAsync(client);
        await ctx.SaveChangesAsync();
        return client;
    }

    protected async Task<Driver> CreateDriverAsync(AppDbContext ctx, Entrepreneur entrepreneur,
        string name = "Test Driver", string dni = "87654321", string license = "A1B2C3D4E5", string contact = "999999999")
    {
        var driver = new Driver(name, dni, license, contact, entrepreneur.Id, entrepreneur);
        await ctx.Set<Driver>().AddAsync(driver);
        await ctx.SaveChangesAsync();
        return driver;
    }

    protected async Task<Vehicle> CreateVehicleAsync(AppDbContext ctx, Entrepreneur entrepreneur,
        string name = "Test Vehicle", string model = "Test Model", string plate = "ABC123",
        string tractorPlate = "TRC456", decimal maxLoad = 20000m, decimal volume = 80m)
    {
        var vehicle = new Vehicle(name, model, plate, tractorPlate, maxLoad, volume, entrepreneur.Id, entrepreneur);
        await ctx.Set<Vehicle>().AddAsync(vehicle);
        await ctx.SaveChangesAsync();
        return vehicle;
    }

    protected async Task<Trip> CreateTripAsync(AppDbContext ctx, string name,
        Driver driver, Vehicle vehicle, Client client, Entrepreneur entrepreneur,
        string type = "Electronics", decimal weight = 100m)
    {
        var trip = new Trip(name, type, weight,
            "Av. Lima 123", DateTime.Now,
            "Av. Peru 456", DateTime.Now.AddHours(3),
            driver.Id, vehicle.Id, client.Id, entrepreneur.Id,
            driver, vehicle, client, entrepreneur);
        await ctx.Set<Trip>().AddAsync(trip);
        await ctx.SaveChangesAsync();
        return trip;
    }

    protected async Task<(AppDbContext dbContext, Driver driver, Vehicle vehicle, Client client, Entrepreneur entrepreneur)>
        CreateTripSetupAsync()
    {
        var dbContext = CreateDbContext();

        var eUser = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, eUser);

        var cUser = await CreateUserAsync(dbContext, "client1", "900000002", false);
        var client = await CreateClientAsync(dbContext, cUser);

        var driver = await CreateDriverAsync(dbContext, entrepreneur);
        var vehicle = await CreateVehicleAsync(dbContext, entrepreneur);

        return (dbContext, driver, vehicle, client, entrepreneur);
    }
}
