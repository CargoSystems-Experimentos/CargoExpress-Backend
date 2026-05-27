using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Domain.Repositories;

public interface IDriverRepository : IBaseRepository<Driver>
{
    Task<IEnumerable<Driver>> FindByEntrepreneurIdAsync(int entrepreneurId);
    Task<Driver?> FindByDniAsync(string dni);
}