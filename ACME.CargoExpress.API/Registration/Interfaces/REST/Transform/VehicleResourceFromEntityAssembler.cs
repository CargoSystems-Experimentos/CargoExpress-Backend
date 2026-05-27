using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class VehicleResourceFromEntityAssembler
{
    public static VehicleResource ToResourceFromEntity(Vehicle entity)
    {
        return new VehicleResource(
            entity.Id,
            entity.Model,
            entity.Plate,
            entity.TractorPlate,
            entity.MaxLoad,
            entity.Volume,
            entity.EntrepreneurId);
    }
}