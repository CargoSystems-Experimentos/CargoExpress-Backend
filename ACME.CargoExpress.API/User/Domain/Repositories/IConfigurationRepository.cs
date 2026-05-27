using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
namespace ACME.CargoExpress.API.User.Domain.Repositories;

public interface IConfigurationRepository : IBaseRepository<Configuration>
{
    Task<Configuration?> FindByUserIdAsync(int userId);
    
}