namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateVehicleResource(string Name, string Model, string Plate, string TractorPlate, decimal MaxLoad, decimal Volume, int EntrepreneurId);