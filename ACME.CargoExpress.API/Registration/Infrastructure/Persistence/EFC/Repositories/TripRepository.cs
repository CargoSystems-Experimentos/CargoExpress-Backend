using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;

public class TripRepository : BaseRepository<Trip>, ITripRepository
{
    private readonly AppDbContext _context;

    public TripRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Trip>> FindByClientIdAsync(int clientId)
    {
        return await _context.Trips.Where(t => t.ClientId == clientId).ToListAsync();
    }
    
    public async Task<IEnumerable<Trip>> FindByEntrepreneurIdAsync(int entrepreneurId)
    {
        return await _context.Trips.Where(t => t.EntrepreneurId == entrepreneurId).ToListAsync();
    }
    
    public async Task<IEnumerable<Client>> FindClientsByEntrepreneurIdAsync(int entrepreneurId)
    {
        return await _context.Trips
            .Where(t => t.EntrepreneurId == entrepreneurId)
            .Select(t => t.Client)
            .Distinct()
            .ToListAsync();
    }

    public async Task<Trip?> FindByNameAsync(string name)
    {
        return await _context.Trips.FirstOrDefaultAsync(t => t.Name == name);
    }
}