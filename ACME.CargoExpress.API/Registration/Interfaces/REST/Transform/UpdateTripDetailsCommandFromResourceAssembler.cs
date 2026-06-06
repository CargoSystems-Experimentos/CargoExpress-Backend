using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateTripDetailsCommandFromResourceAssembler
{
    public static UpdateTripDetailsCommand ToCommandFromResource(UpdateTripDetailsResource resource, int tripId)
    {
        return new UpdateTripDetailsCommand(
            tripId,
            resource.Name,
            resource.Type,
            resource.Weight,
            resource.DriverId,
            resource.VehicleId,
            resource.ClientId);
    }
}
