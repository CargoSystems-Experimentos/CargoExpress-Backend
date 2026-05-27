namespace ACME.CargoExpress.API.User.Domain.Model.Commands;

public record CreateClientCommand(string Name, string Phone, string Dni, int UserId);