namespace ACME.CargoExpress.API.User.Interfaces.REST.Resources;

public record UpdateClientResource(string Name, string Phone, string Dni, int UserId);