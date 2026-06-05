using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class OngoingTripResourceFromEntityAssembler
{
    public static OngoingTripResource ToResourceFromEntity(OngoingTrip entity)
    {
        return new OngoingTripResource(entity.Id, entity.Latitude, entity.Longitude, entity.Speed, entity.Distance, entity.TripId);
    }
}