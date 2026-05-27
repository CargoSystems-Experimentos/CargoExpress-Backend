using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IVehicleCommandService
{
    Task<Vehicle?> Handle(CreateVehicleCommand createVehicleCommand);
    Task<Vehicle?> Handle(UpdateVehicleCommand updateVehicleCommand);
}