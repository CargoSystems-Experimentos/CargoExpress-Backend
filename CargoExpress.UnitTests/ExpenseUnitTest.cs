
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class ExpenseUnitTest
{
    [Fact]
    public async Task GetAll_Expenses_Success()
    {
        // Arrange
        var expense = new Expense(250, "Gasolina", 100, "Viaticos", 50, "Peajes", 1, new Trip());
        var expenseTwo = new Expense(300, "Gasolina", 150, "Viaticos", 100, "Peajes", 2, new Trip());
        var expenses = new List<Expense> { expense, expenseTwo };
        var mockExpenseRepository = new Mock<IExpenseRepository>();
        mockExpenseRepository.Setup(repo => repo.ListAsync()).ReturnsAsync(expenses);

        // Act
        var returnedExpenses = await mockExpenseRepository.Object.ListAsync();

        // Assert
        mockExpenseRepository.Verify(repo => repo.ListAsync(), Times.Once);
        Assert.Equal(expenses, returnedExpenses);
        Assert.Equal(2, returnedExpenses.Count());
    }

    [Fact]
    public async Task GetById_Expense_Success()
    {
        // Arrange
        int validId = 1;
        int invalidId = 0;
        var expense = new Expense(250, "Gasolina", 100, "Viaticos", 50, "Peajes", 1, new Trip());
        var mockExpenseRepository = new Mock<IExpenseRepository>();
        mockExpenseRepository.Setup(repo => repo.FindByIdAsync(validId)).ReturnsAsync(expense);
        mockExpenseRepository.Setup(repo => repo.FindByIdAsync(invalidId)).ReturnsAsync((Expense)null);
        // Act
        var returnedExpense = await mockExpenseRepository.Object.FindByIdAsync(validId);
        var returnedInvalidExpense = await mockExpenseRepository.Object.FindByIdAsync(invalidId);

        // Assert
        mockExpenseRepository.Verify(repo => repo.FindByIdAsync(validId), Times.Once);
        mockExpenseRepository.Verify(repo => repo.FindByIdAsync(invalidId), Times.Once);
        Assert.Equal(expense, returnedExpense);
        Assert.Null(returnedInvalidExpense);
    }

    [Fact]
    public async Task Add_Expense_Success()
    {
        // Arrange
        var expense = new Expense(250, "Gasolina", 100, "Viaticos", 50, "Peajes", 1, new Trip());
        var mockExpenseRepository = new Mock<IExpenseRepository>();
        mockExpenseRepository.Setup(repo => repo.AddAsync(expense)).Returns(Task.CompletedTask);

        // Act
        await mockExpenseRepository.Object.AddAsync(expense);

        // Assert
        mockExpenseRepository.Verify(repo => repo.AddAsync(expense), Times.Once);
    }

    [Fact]
    public void Update_Expense_Success()
    {
        // Arrange
        var expense = new Expense(250, "Gasolina", 100, "Viaticos", 50, "Peajes", 1, new Trip());
        var mockExpenseRepository = new Mock<IExpenseRepository>();
        mockExpenseRepository.Setup(repo => repo.Update(expense));

        // Act
        mockExpenseRepository.Object.Update(expense);

        // Assert
        mockExpenseRepository.Verify(repo => repo.Update(expense), Times.Once);
        Assert.Equal(250, expense.FuelAmount);
        Assert.Equal("Gasolina", expense.FuelDescription);
        Assert.Equal(100, expense.ViaticsAmount);
        Assert.Equal("Viaticos", expense.ViaticsDescription);
        Assert.Equal(50, expense.TollsAmount);
        Assert.Equal("Peajes", expense.TollsDescription);
    }
}