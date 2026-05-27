namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record DriverResource(int Id, string Name, string Dni, string License, string ContactNumber, int EntrepreneurId);