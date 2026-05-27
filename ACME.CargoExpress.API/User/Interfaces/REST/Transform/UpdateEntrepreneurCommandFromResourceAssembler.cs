using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.User.Interfaces.REST.Transform;

public static class UpdateEntrepreneurCommandFromResourceAssembler
{
    public static UpdateEntrepreneurCommand ToCommandFromResource(UpdateEntrepreneurResource resource, int entrepreneurId)
    {
        return new UpdateEntrepreneurCommand(entrepreneurId, resource.Name, resource.Phone, resource.Ruc, 
            resource.UserId, resource.LogoImage);
    }
    
}