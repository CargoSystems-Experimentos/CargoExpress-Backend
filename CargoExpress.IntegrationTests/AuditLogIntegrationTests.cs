using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace CargoExpress.IntegrationTests;

public class AuditLogIntegrationTests : IntegrationTestBase
{
    [Fact]
    public async Task CreateAuditLog_WithValidData_ShouldSucceed()
    {
        var dbContext = CreateDbContext();
        var auditLogRepository = new AuditLogRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);

        var auditLog = new AuditLog("TRIPS", "CREATE", "{\"name\":\"Test Trip\"}", entrepreneur.Id);

        await auditLogRepository.AddAsync(auditLog);
        await unitOfWork.CompleteAsync();

        var retrieved = await auditLogRepository.FindByIdAsync(auditLog.Id);
        Assert.NotNull(retrieved);
        Assert.Equal("TRIPS", retrieved.EntityType);
        Assert.Equal("CREATE", retrieved.Action);
        Assert.Equal(entrepreneur.Id, retrieved.EntrepreneurId);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindAuditLogsByEntrepreneurId_ShouldReturnLogs()
    {
        var dbContext = CreateDbContext();
        var auditLogRepository = new AuditLogRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);

        var log1 = new AuditLog("TRIPS", "CREATE", "{\"name\":\"Trip 1\"}", entrepreneur.Id);
        var log2 = new AuditLog("VEHICLES", "UPDATE", "{\"name\":\"Vehicle 1\"}", entrepreneur.Id);

        await auditLogRepository.AddAsync(log1);
        await auditLogRepository.AddAsync(log2);
        await unitOfWork.CompleteAsync();

        var logs = await auditLogRepository.FindByEntrepreneurIdAsync(entrepreneur.Id);

        Assert.NotNull(logs);
        Assert.Equal(2, logs.Count());

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindAuditLogsByEntrepreneurIdAndEntityType_ShouldFilter()
    {
        var dbContext = CreateDbContext();
        var auditLogRepository = new AuditLogRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);

        var tripLog = new AuditLog("TRIPS", "CREATE", "{\"name\":\"Trip 1\"}", entrepreneur.Id);
        var vehicleLog = new AuditLog("VEHICLES", "CREATE", "{\"name\":\"Vehicle 1\"}", entrepreneur.Id);

        await auditLogRepository.AddAsync(tripLog);
        await auditLogRepository.AddAsync(vehicleLog);
        await unitOfWork.CompleteAsync();

        var tripLogs = await auditLogRepository.FindByEntrepreneurIdAndEntityTypeAsync(entrepreneur.Id, "TRIPS");

        Assert.NotNull(tripLogs);
        Assert.Single(tripLogs);
        Assert.Equal("TRIPS", tripLogs.First().EntityType);

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task ListAuditLogs_ShouldReturnAllLogs()
    {
        var dbContext = CreateDbContext();
        var auditLogRepository = new AuditLogRepository(dbContext);
        var unitOfWork = new UnitOfWork(dbContext);

        var user = await CreateUserAsync(dbContext, "entrepreneur1", "900000001", true);
        var entrepreneur = await CreateEntrepreneurAsync(dbContext, user);

        var log1 = new AuditLog("DRIVERS", "CREATE", "{\"name\":\"Driver 1\"}", entrepreneur.Id);
        var log2 = new AuditLog("EXPENSES", "UPDATE", "{\"amount\":100}", entrepreneur.Id);
        var log3 = new AuditLog("ALERTS", "DELETE", "{\"id\":5}", entrepreneur.Id);

        await auditLogRepository.AddAsync(log1);
        await auditLogRepository.AddAsync(log2);
        await auditLogRepository.AddAsync(log3);
        await unitOfWork.CompleteAsync();

        var logs = await auditLogRepository.ListAsync();

        Assert.NotNull(logs);
        Assert.Equal(3, logs.Count());

        CleanupDatabase(dbContext);
    }

    [Fact]
    public async Task FindAuditLogById_WithInvalidId_ShouldReturnNull()
    {
        var dbContext = CreateDbContext();
        var auditLogRepository = new AuditLogRepository(dbContext);

        var result = await auditLogRepository.FindByIdAsync(Guid.NewGuid());

        Assert.Null(result);

        CleanupDatabase(dbContext);
    }
}
