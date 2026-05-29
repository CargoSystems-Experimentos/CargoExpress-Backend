using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;

namespace ACME.CargoExpress.API.User.Domain.Repositories;

public interface IClientRepository : IBaseRepository<Client>
{
    Task<Client?> FindByDniAsync(string dni);
    Task<Client?> FindByUserIdAsync(int userId);
}