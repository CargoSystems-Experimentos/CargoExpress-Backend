using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class CreateDriverCommandFromResourceAssembler
{
    public static CreateDriverCommand ToCommandFromResource(CreateDriverResource resource)
    {
        return new CreateDriverCommand(resource.Name, resource.Dni, resource.License, resource.ContactNumber, resource.EntrepreneurId);
    }
}