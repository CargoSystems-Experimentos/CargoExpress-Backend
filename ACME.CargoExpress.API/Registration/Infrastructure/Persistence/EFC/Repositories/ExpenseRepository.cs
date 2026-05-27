using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
{
        private readonly AppDbContext _context;

        public ExpenseRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Expense?> FindByTripIdAsync(int tripId)
        {
            return await _context.Expenses.FirstOrDefaultAsync(e => e.TripId == tripId);
        }
}

