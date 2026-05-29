namespace ACME.CargoExpress.API.User.Domain.Model.Commands;

public record UpdateClientCommand(int ClientId, string Name, string Dni, int UserId);
