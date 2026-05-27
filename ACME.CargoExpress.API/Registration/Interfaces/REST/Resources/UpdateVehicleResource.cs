namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateVehicleResource(string Model, string Plate, string TractorPlate, float MaxLoad, float Volume, int EntrepreneurId);