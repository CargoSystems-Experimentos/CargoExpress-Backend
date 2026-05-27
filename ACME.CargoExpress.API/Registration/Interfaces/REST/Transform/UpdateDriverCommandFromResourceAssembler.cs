using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class UpdateDriverCommandFromResourceAssembler
{ 
    public static UpdateDriverCommand ToCommandFromResource(UpdateDriverResource resource, int driverId)
    {
        return new UpdateDriverCommand(driverId, resource.Name, resource.Dni, resource.License, resource.ContactNumber, resource.EntrepreneurId);
    }
}