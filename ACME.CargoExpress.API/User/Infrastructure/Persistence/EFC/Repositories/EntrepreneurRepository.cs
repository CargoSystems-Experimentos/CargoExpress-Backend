using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;

public class EntrepreneurRepository(AppDbContext context)
    : BaseRepository<Entrepreneur>(context), IEntrepreneurRepository
{
    public async Task<Entrepreneur?> FindByUserIdAsync(int userId)
    {
        return await context.Entrepreneurs.FirstOrDefaultAsync(e => e.UserId == userId);
    }

    public async Task<Entrepreneur?> FindByRucAsync(string ruc)
    {
        return await Context.Set<Entrepreneur>()
            .FirstOrDefaultAsync(e => e.Ruc == ruc);
    }

    public async Task<Entrepreneur?> FindByNameAsync(string name)
    {
        return await Context.Set<Entrepreneur>()
            .FirstOrDefaultAsync(e => e.Name == name);
    }
}
