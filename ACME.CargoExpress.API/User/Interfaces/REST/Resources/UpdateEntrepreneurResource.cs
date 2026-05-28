namespace ACME.CargoExpress.API.User.Interfaces.REST.Resources;

public record UpdateEntrepreneurResource(string Name, string Phone, string Ruc, int UserId);
