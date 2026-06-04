using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateVehicleStateCommandFromResourceAssembler
{
    public static UpdateVehicleStateCommand ToCommandFromResource(UpdateVehicleStateResource resource, int vehicleId)
    {
        return new UpdateVehicleStateCommand(vehicleId, resource.State);
    }
}
