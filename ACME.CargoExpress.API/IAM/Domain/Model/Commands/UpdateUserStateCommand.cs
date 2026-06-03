namespace ACME.CargoExpress.API.IAM.Domain.Model.Commands;

public record UpdateUserStateCommand(int UserId, bool State);
