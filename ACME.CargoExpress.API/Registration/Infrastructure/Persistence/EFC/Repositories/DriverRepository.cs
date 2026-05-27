using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;

public class DriverRepository(AppDbContext context) : BaseRepository<Driver>(context), IDriverRepository
{
	public async Task<IEnumerable<Driver>> FindByEntrepreneurIdAsync(int entrepreneurId)
	{
		return await Context.Set<Driver>()
			.Where(d => d.EntrepreneurId == entrepreneurId)
			.ToListAsync();
	}

	public async Task<Driver?> FindByDniAsync(string dni)
	{
		return await Context.Set<Driver>()
			.FirstOrDefaultAsync(d => d.Dni == dni);
	}
}
