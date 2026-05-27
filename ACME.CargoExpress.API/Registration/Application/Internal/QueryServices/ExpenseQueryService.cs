using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;

namespace ACME.CargoExpress.API.Registration.Application.Internal.QueryServices;

public class ExpenseQueryService(IExpenseRepository expenseRepository)
    : IExpenseQueryService
{
    public async Task<Expense?> Handle(GetExpenseByIdQuery query)
    {
        return await expenseRepository.FindByIdAsync(query.ExpenseId);
    }
    
    public async Task<IEnumerable<Expense>> Handle(GetAllExpensesQuery query)
    {
        return await expenseRepository.ListAsync();
    }
}