using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
namespace ACME.CargoExpress.API.Registration.Domain.Repositories;

public interface IOngoingTripRepository : IBaseRepository<OngoingTrip>
{
    Task<OngoingTrip?> FindByTripIdAsync(int tripId);
}