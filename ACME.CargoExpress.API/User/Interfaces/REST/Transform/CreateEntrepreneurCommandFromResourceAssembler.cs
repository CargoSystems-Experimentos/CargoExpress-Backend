using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.User.Interfaces.REST.Transform;

public static class CreateEntrepreneurCommandFromResourceAssembler
{
    public static CreateEntrepreneurCommand ToCommandFromResource(CreateEntrepreneurResource resource)
    {
        return new CreateEntrepreneurCommand(
            resource.Name,
            resource.Phone,
            resource.Ruc,
            resource.UserId);
    }
}
