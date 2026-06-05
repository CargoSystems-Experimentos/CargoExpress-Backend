using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class CreateOngoingTripCommandFromResourceAssembler
{
    public static CreateOngoingTripCommand ToCommandFromResource(CreateOngoingTripResource resource)
    {
        return new CreateOngoingTripCommand(resource.Latitude, resource.Longitude, resource.Speed, resource.Distance, resource.TripId);
    }
}