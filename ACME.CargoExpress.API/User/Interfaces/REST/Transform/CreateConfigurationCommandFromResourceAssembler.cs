using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.User.Interfaces.REST.Transform;

public static class CreateConfigurationCommandFromResourceAssembler
{
    public static CreateConfigurationCommand ToCommandFromResource(CreateConfigurationResource resource)
    {
        return new CreateConfigurationCommand(resource.UserId, resource.Theme, resource.View, resource.AllowDataCollection, resource.UpdateDataSharing);
    }
}