using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IOngoingTripQueryService
{
    Task<OngoingTrip?> Handle(GetOnGoingTripByIdQuery query);
    Task<IEnumerable<OngoingTrip>> Handle(GetAllOngoingTripsQuery query);
}