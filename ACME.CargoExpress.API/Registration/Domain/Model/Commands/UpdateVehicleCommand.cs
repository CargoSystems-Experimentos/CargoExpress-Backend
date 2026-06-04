namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateVehicleCommand(int VehicleId, string Name, string Model, string Plate, string TractorPlate, decimal MaxLoad, decimal Volume, int EntrepreneurId);