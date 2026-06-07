using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;

public class ClientRepository(AppDbContext context) 
    : BaseRepository<Client>(context), IClientRepository
{
    public async Task<Client?> FindByUserIdAsync(int userId)
    {
        return await context.Clients.FirstOrDefaultAsync(c => c.UserId == userId);
    }
    public async Task<Client?> FindByDniAsync(string dni)
    {
        return await Context.Set<Client>()
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Dni == dni);
    }
}