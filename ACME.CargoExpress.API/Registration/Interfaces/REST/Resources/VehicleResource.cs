namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record VehicleResource(int Id, string Name, string Model, string Plate, string TractorPlate, float MaxLoad, float Volume, int EntrepreneurId);