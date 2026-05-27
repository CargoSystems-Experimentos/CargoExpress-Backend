namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record CreateDriverResource(string Name, string Dni, string License, string ContactNumber, int EntrepreneurId);