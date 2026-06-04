namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record CreateVehicleResource(string Name, string Model, string Plate, string TractorPlate, decimal MaxLoad, decimal Volume, int EntrepreneurId);