using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class ExpenseIntegrationTests : IntegrationTestBase
{
    private static Trip BuildTrip(string name = "Test Trip") => new Trip
    {
        Name = name,
        Type = "Electronics",
        Weight = 100,
        LoadLocation = "Lima",
        LoadDate = DateTime.Now,
        UnloadLocation = "Callao",
        UnloadDate = DateTime.Now.AddHours(2),
        DriverId = 1,
        VehicleId = 1,
        ClientId = 1,
        EntrepreneurId = 1
    };

    [Fact]
    public async Task CreateExpense_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);
        var expenseRepository = new ExpenseRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = BuildTrip();
        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        var expense = new Expense(200, "Gasolina", 50, "Viaticos dia", 30, "Peaje norte", trip.Id, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();

        var retrieved = await expenseRepository.FindByIdAsync(expense.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(200, retrieved.FuelAmount);
        Assert.Equal("Gasolina", retrieved.FuelDescription);
        Assert.Equal(50, retrieved.ViaticsAmount);
        Assert.Equal(30, retrieved.TollsAmount);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindExpenseByTripId_ShouldReturnExpense()
    {
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);
        var expenseRepository = new ExpenseRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = BuildTrip("Trip with Expense");
        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        var expense = new Expense(150, "Diesel", 75, "Hotel", 20, "Peaje sur", trip.Id, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();

        var found = await expenseRepository.FindByTripIdAsync(trip.Id);

        Assert.NotNull(found);
        Assert.Equal(150, found.FuelAmount);
        Assert.Equal("Diesel", found.FuelDescription);
        Assert.Equal(trip.Id, found.TripId);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindExpenseByTripId_WithNoExpense_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var expenseRepository = new ExpenseRepository(dbContext);

        var found = await expenseRepository.FindByTripIdAsync(999);

        Assert.Null(found);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task UpdateExpense_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var tripRepository = new TripRepository(dbContext);
        var expenseRepository = new ExpenseRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var trip = BuildTrip("Update Expense Trip");
        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();

        var expense = new Expense(100, "Original fuel", 40, "Original viatics", 10, "Original toll", trip.Id, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();

        expense.FuelAmount = 250;
        expense.FuelDescription = "Updated fuel";
        expense.TollsAmount = 60;
        expenseRepository.Update(expense);
        await unitOfWork.CompleteAsync();

        var updated = await expenseRepository.FindByIdAsync(expense.Id);
        Assert.NotNull(updated);
        Assert.Equal(250, updated.FuelAmount);
        Assert.Equal("Updated fuel", updated.FuelDescription);
        Assert.Equal(60, updated.TollsAmount);

        CleanupDatabase(dbContext);
    }
}
