namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateAlertCommand(int AlertId, string Title, string Type, string Description, DateTime Date);
