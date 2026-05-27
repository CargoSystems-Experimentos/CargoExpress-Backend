using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface ITripCommandService
{
    Task<Trip?> Handle(CreateTripCommand createTripCommand);
    Task<Trip?> Handle(UpdateTripCommand updateTripCommand);
}