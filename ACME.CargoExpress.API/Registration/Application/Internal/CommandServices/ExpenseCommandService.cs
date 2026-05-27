 using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
 using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class ExpenseCommandService(IExpenseRepository expenseRepository ,ITripRepository tripRepository, IUnitOfWork unitOfWork)
    : IExpenseCommandService
{
    public async Task<Expense?> Handle(CreateExpenseCommand command)
    {
        // Additional validation to check if the trip exists
        var trip = await tripRepository.FindByIdAsync(command.TripId);
        if (trip == null)
        {
            throw new ArgumentException("TripId not found.");
        }

        // Check if an Expense with the same TripId already exists
        var existingExpense = await expenseRepository.FindByTripIdAsync(command.TripId);
        if (existingExpense != null)
        {
            throw new InvalidOperationException("An Expense with the same TripId already exists.");
        }

        var expense = new Expense(command, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();
        return expense;
    }
    
    public async Task<Expense?> Handle(UpdateExpenseCommand command)
    {
        var expense = await expenseRepository.FindByIdAsync(command.ExpenseId);
        if (expense == null)
        {
            return null;
        }
        //Update the expense information
        expense.ViaticsAmount = command.ViaticsAmount;
        expense.ViaticsDescription = command.ViaticsDescription;
        expense.FuelAmount = command.FuelAmount;
        expense.FuelDescription = command.FuelDescription;
        expense.TollsAmount = command.TollsAmount;
        expense.TollsDescription = command.TollsDescription;
    
        await unitOfWork.CompleteAsync();
        return expense;
    }
    
    
}
 