namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateDriverResource(string Name, string Dni, string License, string ContactNumber, int EntrepreneurId);