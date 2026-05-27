using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;

namespace ACME.CargoExpress.API.Registration.Application.Internal.QueryServices;

public class OngoingTripQueryService(IOngoingTripRepository ongoingTripRepository)
    : IOngoingTripQueryService
{
    public async Task<OngoingTrip?> Handle(GetOnGoingTripByIdQuery query)
    {
        return await ongoingTripRepository.FindByIdAsync(query.OngoingTripId);
    }
    
    public async Task<IEnumerable<OngoingTrip>> Handle(GetAllOngoingTripsQuery query)
    {
        return await ongoingTripRepository.ListAsync();
    }
}
