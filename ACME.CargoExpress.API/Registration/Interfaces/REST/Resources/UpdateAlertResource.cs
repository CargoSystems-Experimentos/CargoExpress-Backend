namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateAlertResource(string Title, string Type, string Description, DateTime Date);
