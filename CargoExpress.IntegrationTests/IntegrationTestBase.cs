using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace CargoExpress.IntegrationTests;

public abstract class IntegrationTestBase
{
    protected AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        var dbContext = new AppDbContext(options);
        dbContext.Database.OpenConnection();
        dbContext.Database.EnsureCreated();
        
        return dbContext;
    }

    protected void CleanupDatabase(AppDbContext dbContext)
    {
        dbContext.Database.EnsureDeleted();
        dbContext.Dispose();
    }
}
