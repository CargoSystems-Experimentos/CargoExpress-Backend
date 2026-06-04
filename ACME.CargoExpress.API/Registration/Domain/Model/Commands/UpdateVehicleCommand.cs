namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateVehicleCommand(int VehicleId, string Name);