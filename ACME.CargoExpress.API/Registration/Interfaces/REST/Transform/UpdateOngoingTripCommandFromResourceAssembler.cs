using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
// using value object enum removed; State is a string

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateOngoingTripCommandFromResourceAssembler
{
    public static UpdateOngoingTripCommand ToCommandFromResource(UpdateOngoingTripResource resource, int ongoingTripId)
    {
        var state = string.IsNullOrWhiteSpace(resource.State) ? "PENDIENTE" : resource.State.Replace(" ", "_").ToUpperInvariant();
        return new UpdateOngoingTripCommand(ongoingTripId, state, resource.Latitude, resource.Longitude, resource.Speed, resource.Distance, resource.TripId);
    }
}