using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class CreateExpenseCommandFromResourceAssembler
{
    public static CreateExpenseCommand ToCommandFromResource(CreateExpenseResource resource)
    {
        return new CreateExpenseCommand(resource.FuelAmount, resource.FuelDescription, resource.ViaticsAmount,
            resource.ViaticsDescription, resource.TollsAmount, resource.TollsDescription, resource.TripId);
    }
}