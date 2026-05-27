namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateVehicleCommand(int VehicleId, string Model, string Plate, string TractorPlate, float MaxLoad, float Volume, int EntrepreneurId);