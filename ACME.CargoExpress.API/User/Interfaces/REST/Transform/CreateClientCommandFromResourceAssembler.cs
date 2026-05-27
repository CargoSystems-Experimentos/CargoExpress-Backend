using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.User.Interfaces.REST.Transform;

public static class CreateClientCommandFromResourceAssembler
{
    public static CreateClientCommand ToCommandFromResource(CreateClientResource resource)
    {
        return new CreateClientCommand(
            resource.Name,
            resource.Phone,
            resource.Dni,
            resource.UserId);
    }
    
}