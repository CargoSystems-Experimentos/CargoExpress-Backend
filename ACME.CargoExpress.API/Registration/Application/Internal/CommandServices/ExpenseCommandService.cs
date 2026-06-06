 using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
 using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class ExpenseCommandService(IExpenseRepository expenseRepository, ITripRepository tripRepository, IUnitOfWork unitOfWork)
    : IExpenseCommandService
{
    public async Task<Expense?> Handle(CreateExpenseCommand command)
    {
        if (command.FuelAmount <= 0)
            throw new ArgumentException("El monto de combustible no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(command.FuelDescription))
            throw new ArgumentException("La descripción de combustible no puede estar vacía.");
        if (command.FuelDescription.Length > 200)
            throw new ArgumentException("La descripción de combustible no puede superar los 200 caracteres.");

        if (command.ViaticsAmount <= 0)
            throw new ArgumentException("El monto de viáticos no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(command.ViaticsDescription))
            throw new ArgumentException("La descripción de viáticos no puede estar vacía.");
        if (command.ViaticsDescription.Length > 200)
            throw new ArgumentException("La descripción de viáticos no puede superar los 200 caracteres.");

        if (command.TollsAmount <= 0)
            throw new ArgumentException("El monto de peajes no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(command.TollsDescription))
            throw new ArgumentException("La descripción de peajes no puede estar vacía.");
        if (command.TollsDescription.Length > 200)
            throw new ArgumentException("La descripción de peajes no puede superar los 200 caracteres.");

        var trip = await tripRepository.FindByIdAsync(command.TripId);
        if (trip == null)
            throw new ArgumentException("No se ha encontrado el viaje indicado.");

        if (trip.State == "CANCELED")
            throw new InvalidOperationException("No se pueden realizar operaciones en un viaje cancelado.");

        var existingExpense = await expenseRepository.FindByTripIdAsync(command.TripId);
        if (existingExpense != null)
            throw new InvalidOperationException("Ya existe un gasto asociado al viaje indicado.");

        var expense = new Expense(command, trip);
        await expenseRepository.AddAsync(expense);
        await unitOfWork.CompleteAsync();
        return expense;
    }

    public async Task<Expense?> Handle(UpdateExpenseCommand command)
    {
        var expense = await expenseRepository.FindByIdAsync(command.ExpenseId);
        if (expense == null) return null;

        if (!expense.State)
            throw new InvalidOperationException("No se puede modificar un gasto inactivo.");

        var trip = await tripRepository.FindByIdAsync(command.TripId);
        if (trip == null)
            throw new ArgumentException("No se ha encontrado el viaje indicado.");

        if (trip.State == "CANCELED")
            throw new InvalidOperationException("No se pueden realizar operaciones en un viaje cancelado.");

        if (command.FuelAmount <= 0)
            throw new ArgumentException("El monto de combustible no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(command.FuelDescription))
            throw new ArgumentException("La descripción de combustible no puede estar vacía.");
        if (command.FuelDescription.Length > 200)
            throw new ArgumentException("La descripción de combustible no puede superar los 200 caracteres.");

        if (command.ViaticsAmount <= 0)
            throw new ArgumentException("El monto de viáticos no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(command.ViaticsDescription))
            throw new ArgumentException("La descripción de viáticos no puede estar vacía.");
        if (command.ViaticsDescription.Length > 200)
            throw new ArgumentException("La descripción de viáticos no puede superar los 200 caracteres.");

        if (command.TollsAmount <= 0)
            throw new ArgumentException("El monto de peajes no puede estar vacío.");
        if (string.IsNullOrWhiteSpace(command.TollsDescription))
            throw new ArgumentException("La descripción de peajes no puede estar vacía.");
        if (command.TollsDescription.Length > 200)
            throw new ArgumentException("La descripción de peajes no puede superar los 200 caracteres.");

        expense.FuelAmount = command.FuelAmount;
        expense.FuelDescription = command.FuelDescription;
        expense.ViaticsAmount = command.ViaticsAmount;
        expense.ViaticsDescription = command.ViaticsDescription;
        expense.TollsAmount = command.TollsAmount;
        expense.TollsDescription = command.TollsDescription;

        await unitOfWork.CompleteAsync();
        return expense;
    }

    public async Task<Expense?> Handle(UpdateExpenseStateCommand command)
    {
        var expense = await expenseRepository.FindByIdAsync(command.ExpenseId);
        if (expense == null) return null;

        expense.State = command.State;
        await unitOfWork.CompleteAsync();
        return expense;
    }
}
