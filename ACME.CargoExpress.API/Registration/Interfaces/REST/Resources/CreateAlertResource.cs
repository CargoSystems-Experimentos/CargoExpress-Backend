namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record CreateAlertResource(string Title, string Description, DateTime Date, int TripId);