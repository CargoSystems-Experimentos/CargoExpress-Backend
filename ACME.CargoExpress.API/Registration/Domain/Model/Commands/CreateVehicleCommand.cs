namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record CreateVehicleCommand(string Model, string Plate, string TractorPlate, float MaxLoad, float Volume, int EntrepreneurId);