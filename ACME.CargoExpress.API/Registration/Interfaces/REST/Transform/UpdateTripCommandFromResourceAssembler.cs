using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateTripCommandFromResourceAssembler
{
    public static UpdateTripCommand ToCommandFromResource(UpdateTripResource resource, int tripId)
    {
        return new UpdateTripCommand(
            tripId,
            resource.Name,
            resource.Type,
            resource.Weight,
            resource.LoadLocation,
            resource.LoadDate,
            resource.UnloadLocation,
            resource.UnloadDate,
            resource.DriverId,
            resource.VehicleId,
            resource.ClientId,
            resource.EntrepreneurId,
            resource.EvidenceImg);
    }
}