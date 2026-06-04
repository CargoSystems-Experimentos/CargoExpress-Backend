using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateDriverStateCommandFromResourceAssembler
{
    public static UpdateDriverStateCommand ToCommandFromResource(UpdateDriverStateResource resource, int driverId)
    {
        return new UpdateDriverStateCommand(driverId, resource.State);
    }
}
