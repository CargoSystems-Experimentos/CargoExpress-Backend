using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;

/// <summary>
/// AuditLog uses a Guid primary key, so it does not extend the int-keyed BaseRepository.
/// </summary>
public class AuditLogRepository(AppDbContext context) : IAuditLogRepository
{
    public async Task AddAsync(AuditLog auditLog) => await context.Set<AuditLog>().AddAsync(auditLog);

    public async Task<AuditLog?> FindByIdAsync(Guid id) => await context.Set<AuditLog>().FindAsync(id);

    public async Task<IEnumerable<AuditLog>> ListAsync() =>
        await context.Set<AuditLog>()
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();

    public async Task<IEnumerable<AuditLog>> FindByEntrepreneurIdAsync(int entrepreneurId) =>
        await context.Set<AuditLog>()
            .Where(a => a.EntrepreneurId == entrepreneurId)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();

    public async Task<IEnumerable<AuditLog>> FindByEntrepreneurIdAndEntityTypeAsync(int entrepreneurId, string entityType) =>
        await context.Set<AuditLog>()
            .Where(a => a.EntrepreneurId == entrepreneurId && a.EntityType == entityType)
            .OrderByDescending(a => a.Timestamp)
            .ToListAsync();
}
