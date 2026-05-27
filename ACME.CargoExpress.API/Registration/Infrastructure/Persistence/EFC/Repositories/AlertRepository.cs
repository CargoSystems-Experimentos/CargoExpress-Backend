using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;

public class AlertRepository : BaseRepository<Alert>, IAlertRepository
{
    private readonly AppDbContext _context;

    public AlertRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Alert>> FindByOngoingTripIdAsync(int ongoingTripId)
    {
        return await _context.Alerts.Where(a => a.OngoingTripId == ongoingTripId).ToListAsync();
    }

    public async Task<Alert?> FindByTitleAsync(string title)
    {
        return await _context.Alerts.FirstOrDefaultAsync(a => a.Title == title);
    }
}