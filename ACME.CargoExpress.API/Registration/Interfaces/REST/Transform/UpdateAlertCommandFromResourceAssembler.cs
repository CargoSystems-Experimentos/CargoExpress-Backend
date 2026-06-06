using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateAlertCommandFromResourceAssembler
{
    public static UpdateAlertCommand ToCommandFromResource(UpdateAlertResource resource, int alertId)
    {
        return new UpdateAlertCommand(alertId, resource.Title, resource.Type, resource.Description, resource.Date);
    }
}
