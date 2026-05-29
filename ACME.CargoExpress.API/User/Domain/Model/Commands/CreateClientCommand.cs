namespace ACME.CargoExpress.API.User.Domain.Model.Commands;

public record CreateClientCommand(string Name, string Dni, DateTime BirthDate, int UserId);
