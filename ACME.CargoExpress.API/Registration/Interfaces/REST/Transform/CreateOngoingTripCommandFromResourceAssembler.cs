using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class CreateOngoingTripCommandFromResourceAssembler
{
    public static CreateOngoingTripCommand ToCommandFromResource(CreateOngoingTripResource resource)
    {
        // Mantener el valor como string; si es null/empty usar PENDIENTE
        var state = string.IsNullOrWhiteSpace(resource.State) ? "PENDIENTE" : resource.State.Replace(" ", "_").ToUpperInvariant();
        return new CreateOngoingTripCommand(state, resource.Latitude, resource.Longitude, resource.Speed, resource.Distance, resource.TripId);
    }
}