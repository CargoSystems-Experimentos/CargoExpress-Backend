using ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using Moq;

namespace CargoExpress.UnitTests;

public class ExpenseUnitTest
{
    // ---------------------------------------------------------------
    // Existing repository-mock tests (preserved)
    // ---------------------------------------------------------------

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
        Assert.True(expense.State);
    }

    [Fact]
    public void Create_Expense_DefaultState_IsTrue()
    {
        // Arrange & Act
        var expense = new Expense(250, "Gasolina", 100, "Viaticos", 50, "Peajes", 1, new Trip());

        // Assert
        Assert.True(expense.State);
    }

    [Fact]
    public void Update_ExpenseState_Success()
    {
        // Arrange
        var expense = new Expense(250, "Gasolina", 100, "Viaticos", 50, "Peajes", 1, new Trip());
        var mockExpenseRepository = new Mock<IExpenseRepository>();
        mockExpenseRepository.Setup(repo => repo.Update(expense));

        // Act
        expense.State = false;
        mockExpenseRepository.Object.Update(expense);

        // Assert
        mockExpenseRepository.Verify(repo => repo.Update(expense), Times.Once);
        Assert.False(expense.State);
    }

    // ---------------------------------------------------------------
    // ExpenseCommandService tests
    // ---------------------------------------------------------------

    private (ExpenseCommandService service, Mock<IExpenseRepository> expenseRepo, Mock<ITripRepository> tripRepo)
        BuildService(Trip? trip = null, Expense? existingExpense = null)
    {
        var mockExpenseRepo = new Mock<IExpenseRepository>();
        var mockTripRepo = new Mock<ITripRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();

        var defaultTrip = trip ?? new Trip { State = "AWAITING" };
        mockTripRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(defaultTrip);
        mockExpenseRepo.Setup(r => r.FindByTripIdAsync(It.IsAny<int>())).ReturnsAsync(existingExpense);
        mockExpenseRepo.Setup(r => r.AddAsync(It.IsAny<Expense>())).Returns(Task.CompletedTask);
        mockUnitOfWork.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

        var service = new ExpenseCommandService(
            mockExpenseRepo.Object,
            mockTripRepo.Object,
            mockUnitOfWork.Object);

        return (service, mockExpenseRepo, mockTripRepo);
    }

    private static CreateExpenseCommand ValidCreateCommand(int tripId = 1) =>
        new CreateExpenseCommand(250m, "Gasolina regular", 100m, "Alimentación diaria", 50m, "Peajes autopista", tripId);

    // --- Valid creation ---

    [Fact]
    public async Task Create_Expense_WithValidData_ReturnsExpense()
    {
        var (service, _, _) = BuildService();
        var result = await service.Handle(ValidCreateCommand());

        Assert.NotNull(result);
        Assert.True(result!.State);
    }

    // --- FuelAmount validation ---

    [Fact]
    public async Task Create_Expense_WithZeroFuelAmount_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateExpenseCommand(0m, "Gasolina regular", 100m, "Alimentación", 50m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("combustible", ex.Message);
    }

    // --- FuelDescription validation ---

    [Fact]
    public async Task Create_Expense_WithEmptyFuelDescription_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateExpenseCommand(250m, "", 100m, "Alimentación", 50m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("combustible", ex.Message);
    }

    [Fact]
    public async Task Create_Expense_WithFuelDescriptionTooLong_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var longDesc = new string('A', 201);
        var command = new CreateExpenseCommand(250m, longDesc, 100m, "Alimentación", 50m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("combustible", ex.Message);
    }

    // --- ViaticsAmount validation ---

    [Fact]
    public async Task Create_Expense_WithZeroViaticsAmount_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateExpenseCommand(250m, "Gasolina", 0m, "Alimentación", 50m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("viáticos", ex.Message);
    }

    // --- ViaticsDescription validation ---

    [Fact]
    public async Task Create_Expense_WithEmptyViaticsDescription_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateExpenseCommand(250m, "Gasolina", 100m, "", 50m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("viáticos", ex.Message);
    }

    // --- TollsAmount validation ---

    [Fact]
    public async Task Create_Expense_WithZeroTollsAmount_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateExpenseCommand(250m, "Gasolina", 100m, "Alimentación", 0m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("peajes", ex.Message);
    }

    // --- TollsDescription validation ---

    [Fact]
    public async Task Create_Expense_WithEmptyTollsDescription_ThrowsArgumentException()
    {
        var (service, _, _) = BuildService();
        var command = new CreateExpenseCommand(250m, "Gasolina", 100m, "Alimentación", 50m, "", 1);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(command));
        Assert.Contains("peajes", ex.Message);
    }

    // --- Trip not found ---

    [Fact]
    public async Task Create_Expense_WithNonExistentTripId_ThrowsArgumentException()
    {
        var mockExpenseRepo = new Mock<IExpenseRepository>();
        var mockTripRepo = new Mock<ITripRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockTripRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Trip?)null);
        var service = new ExpenseCommandService(mockExpenseRepo.Object, mockTripRepo.Object, mockUnitOfWork.Object);

        var ex = await Assert.ThrowsAsync<ArgumentException>(() => service.Handle(ValidCreateCommand(99)));
        Assert.Contains("viaje", ex.Message);
    }

    // --- Canceled trip ---

    [Fact]
    public async Task Create_Expense_WhenTripIsCanceled_ThrowsInvalidOperationException()
    {
        var canceledTrip = new Trip { State = "CANCELED" };
        var (service, _, _) = BuildService(canceledTrip);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Handle(ValidCreateCommand()));
        Assert.Contains("cancelado", ex.Message);
    }

    // --- Existing expense for trip ---

    [Fact]
    public async Task Create_Expense_WhenExpenseAlreadyExists_ThrowsInvalidOperationException()
    {
        var existingExpense = new Expense(100m, "Old fuel", 50m, "Old viatics", 20m, "Old tolls", 1, new Trip());
        var (service, _, _) = BuildService(existingExpense: existingExpense);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Handle(ValidCreateCommand()));
        Assert.Contains("gasto", ex.Message);
    }

    // --- UpdateExpenseCommand: expense not found returns null ---

    [Fact]
    public async Task Update_Expense_WhenNotFound_ReturnsNull()
    {
        var mockExpenseRepo = new Mock<IExpenseRepository>();
        var mockTripRepo = new Mock<ITripRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockExpenseRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Expense?)null);
        var service = new ExpenseCommandService(mockExpenseRepo.Object, mockTripRepo.Object, mockUnitOfWork.Object);

        var command = new UpdateExpenseCommand(99, 250m, "Gasolina", 100m, "Alimentación", 50m, "Peajes", 1);
        var result = await service.Handle(command);

        Assert.Null(result);
    }

    // --- UpdateExpenseCommand: inactive expense ---

    [Fact]
    public async Task Update_Expense_WhenInactive_ThrowsInvalidOperationException()
    {
        var inactiveExpense = new Expense(250m, "Gasolina", 100m, "Alimentación", 50m, "Peajes", 1, new Trip());
        inactiveExpense.State = false;

        var mockExpenseRepo = new Mock<IExpenseRepository>();
        var mockTripRepo = new Mock<ITripRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockExpenseRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(inactiveExpense);
        var service = new ExpenseCommandService(mockExpenseRepo.Object, mockTripRepo.Object, mockUnitOfWork.Object);

        var command = new UpdateExpenseCommand(1, 300m, "Diesel", 120m, "Alimentación", 60m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Handle(command));
        Assert.Contains("inactivo", ex.Message);
    }

    // --- UpdateExpenseCommand: canceled trip ---

    [Fact]
    public async Task Update_Expense_WhenTripIsCanceled_ThrowsInvalidOperationException()
    {
        var activeExpense = new Expense(250m, "Gasolina", 100m, "Alimentación", 50m, "Peajes", 1, new Trip());

        var canceledTrip = new Trip { State = "CANCELED" };

        var mockExpenseRepo = new Mock<IExpenseRepository>();
        var mockTripRepo = new Mock<ITripRepository>();
        var mockUnitOfWork = new Mock<IUnitOfWork>();
        mockExpenseRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(activeExpense);
        mockTripRepo.Setup(r => r.FindByIdAsync(It.IsAny<int>())).ReturnsAsync(canceledTrip);
        var service = new ExpenseCommandService(mockExpenseRepo.Object, mockTripRepo.Object, mockUnitOfWork.Object);

        var command = new UpdateExpenseCommand(1, 300m, "Diesel", 120m, "Alimentación", 60m, "Peajes", 1);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => service.Handle(command));
        Assert.Contains("cancelado", ex.Message);
    }
}
