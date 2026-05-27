using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;

namespace ACME.CargoExpress.API.User.Domain.Repositories;

public interface IEntrepreneurRepository : IBaseRepository<Entrepreneur>
{
    Task<Entrepreneur?> FindByUserIdAsync(int userId);
    Task<Entrepreneur?> FindByPhoneAsync(string phone);
}