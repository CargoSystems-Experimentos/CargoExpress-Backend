namespace ACME.CargoExpress.API.User.Interfaces.REST.Resources;

public record UpdateClientResource(string Name, string Dni, DateTime BirthDate, int UserId);
