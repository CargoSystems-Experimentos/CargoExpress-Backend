using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IExpenseCommandService
{
    Task<Expense?> Handle(CreateExpenseCommand createTripCommand);
    Task<Expense?> Handle(UpdateExpenseCommand updateTripCommand);
}