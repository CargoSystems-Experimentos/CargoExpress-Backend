using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class ExpenseIntegrationTests : IntegrationTestBase
{
    private async Task<(ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration.AppDbContext dbContext, Trip trip)>
        SetupTripAsync(string tripName = "Test Trip")
    {
        var (dbContext, driver, vehicle, client, entrepreneur) = await CreateTripSetupAsync();
        var trip = await CreateTripAsync(dbContext, tripName, driver, vehicle, client, entrepreneur);
        return (dbContext, trip);
    }

    [Fact]
    public async Task CreateExpense_WithValidData_ShouldSucceed()
    {
        var (dbContext, trip) = await SetupTripAsync();
        var expenseRepository = new ExpenseRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var expense = new Expense(200m, "Gasolina", 50m, "Viaticos dia", 30m, "Peaje norte", trip.Id, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();

        var retrieved = await expenseRepository.FindByIdAsync(expense.Id);
        Assert.NotNull(retrieved);
        Assert.Equal(200m, retrieved.FuelAmount);
        Assert.Equal("Gasolina", retrieved.FuelDescription);
        Assert.Equal(50m, retrieved.ViaticsAmount);
        Assert.Equal(30m, retrieved.TollsAmount);
        Assert.True(retrieved.State);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindExpenseByTripId_ShouldReturnExpense()
    {
        var (dbContext, trip) = await SetupTripAsync("Trip with Expense");
        var expenseRepository = new ExpenseRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var expense = new Expense(150m, "Diesel", 75m, "Hotel", 20m, "Peaje sur", trip.Id, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();

        var found = await expenseRepository.FindByTripIdAsync(trip.Id);

        Assert.NotNull(found);
        Assert.Equal(150m, found.FuelAmount);
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
        var (dbContext, trip) = await SetupTripAsync("Update Expense Trip");
        var expenseRepository = new ExpenseRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var expense = new Expense(100m, "Original fuel", 40m, "Original viatics", 10m, "Original toll", trip.Id, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();

        expense.FuelAmount = 250m;
        expense.FuelDescription = "Updated fuel";
        expense.TollsAmount = 60m;
        expenseRepository.Update(expense);
        await unitOfWork.CompleteAsync();

        var updated = await expenseRepository.FindByIdAsync(expense.Id);
        Assert.NotNull(updated);
        Assert.Equal(250m, updated.FuelAmount);
        Assert.Equal("Updated fuel", updated.FuelDescription);
        Assert.Equal(60m, updated.TollsAmount);

        CleanupDatabase(dbContext);
    }
}
