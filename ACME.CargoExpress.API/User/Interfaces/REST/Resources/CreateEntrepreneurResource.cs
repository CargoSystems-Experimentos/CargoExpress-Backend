namespace ACME.CargoExpress.API.User.Interfaces.REST.Resources;

public record CreateEntrepreneurResource(string Name, string Phone, string Ruc, int UserId);
