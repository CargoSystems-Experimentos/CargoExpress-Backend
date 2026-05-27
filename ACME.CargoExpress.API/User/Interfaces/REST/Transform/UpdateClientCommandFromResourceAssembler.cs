using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.User.Interfaces.REST.Transform;

public static class UpdateClientCommandFromResourceAssembler
{
    public static UpdateClientCommand ToCommandFromResource(UpdateClientResource resource, int clientId)
    {
        return new UpdateClientCommand(clientId, resource.Name, resource.Phone, resource.Dni, resource.UserId);
    }
    
}