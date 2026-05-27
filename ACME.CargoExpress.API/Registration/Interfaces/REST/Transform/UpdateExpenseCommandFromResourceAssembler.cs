using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateExpenseCommandFromResourceAssembler
{
    public static UpdateExpenseCommand ToCommandFromResource(UpdateExpenseResource resource, int expenseId)
    {
        return new UpdateExpenseCommand(expenseId, resource.FuelAmount, resource.FuelDescription, resource.ViaticsAmount, resource.ViaticsDescription,
            resource.TollsAmount, resource.TollsDescription, resource.TripId);
    }
}