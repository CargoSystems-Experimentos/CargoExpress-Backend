using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateOngoingTripCommandFromResourceAssembler
{
    public static UpdateOngoingTripCommand ToCommandFromResource(UpdateOngoingTripResource resource, int ongoingTripId)
    {
        return new UpdateOngoingTripCommand(ongoingTripId, resource.Latitude, resource.Longitude, resource.Speed, resource.Distance, resource.TripId);
    }
}