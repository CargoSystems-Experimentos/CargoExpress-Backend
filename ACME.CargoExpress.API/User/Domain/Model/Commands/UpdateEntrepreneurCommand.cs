namespace ACME.CargoExpress.API.User.Domain.Model.Commands;

public record UpdateEntrepreneurCommand(int EntrepreneurId, string Name, string Ruc, int UserId);
