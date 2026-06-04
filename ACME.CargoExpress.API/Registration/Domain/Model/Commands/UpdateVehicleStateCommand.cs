namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateVehicleStateCommand(int VehicleId, string State);
