using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.User.Infrastructure.Persistence.EFC.Repositories;

public class ConfigurationRepository(AppDbContext context)
    : BaseRepository<Configuration>(context), IConfigurationRepository
{
    public async Task<Configuration?> FindByUserIdAsync(int userId)
    {
        return await context.Configurations.FirstOrDefaultAsync(c => c.UserId == userId);
    }
}
