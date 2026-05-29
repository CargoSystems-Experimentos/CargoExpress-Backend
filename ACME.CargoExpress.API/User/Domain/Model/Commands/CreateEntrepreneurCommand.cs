namespace ACME.CargoExpress.API.User.Domain.Model.Commands;

public record CreateEntrepreneurCommand(string Name, string Ruc, int UserId);
