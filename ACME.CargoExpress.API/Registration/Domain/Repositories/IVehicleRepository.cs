using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Domain.Repositories;

public interface IVehicleRepository: IBaseRepository<Vehicle>
{
	Task<IEnumerable<Vehicle>> FindByEntrepreneurIdAsync(int entrepreneurId);
	Task<Vehicle?> FindByPlateAsync(string plate);
}