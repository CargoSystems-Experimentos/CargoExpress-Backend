using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.Registration.Infrastructure.Persistence.EFC.Repositories;

public class VehicleRepository(AppDbContext context): BaseRepository<Vehicle>(context), IVehicleRepository
{
	public async Task<IEnumerable<Vehicle>> FindByEntrepreneurIdAsync(int entrepreneurId)
	{
		return await Context.Set<Vehicle>()
			.Where(v => v.EntrepreneurId == entrepreneurId)
			.ToListAsync();
	}

	public async Task<Vehicle?> FindByPlateAsync(string plate)
	{
		return await Context.Set<Vehicle>()
			.FirstOrDefaultAsync(v => v.Plate == plate);
	}
}
