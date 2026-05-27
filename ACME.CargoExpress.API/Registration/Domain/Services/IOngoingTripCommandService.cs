using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IOngoingTripCommandService
{
    Task<OngoingTrip?> Handle(CreateOngoingTripCommand createTripCommand);
    Task<OngoingTrip?> Handle(UpdateOngoingTripCommand updateTripCommand);
}