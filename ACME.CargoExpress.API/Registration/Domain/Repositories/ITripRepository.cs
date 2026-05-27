using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;

namespace ACME.CargoExpress.API.Registration.Domain.Repositories;

public interface ITripRepository : IBaseRepository<Trip>
{
    Task<IEnumerable<Trip>> FindByClientIdAsync(int clientId);
    Task<IEnumerable<Trip>> FindByEntrepreneurIdAsync(int entrepreneurId);
    Task<IEnumerable<Client>> FindClientsByEntrepreneurIdAsync(int entrepreneurId);
    Task<Trip?> FindByNameAsync(string name);
}