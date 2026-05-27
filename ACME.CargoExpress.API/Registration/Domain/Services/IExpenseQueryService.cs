using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IExpenseQueryService
{
    Task<Expense?> Handle(GetExpenseByIdQuery query);
    Task<IEnumerable<Expense>> Handle(GetAllExpensesQuery query);
}