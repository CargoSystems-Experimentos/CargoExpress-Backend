namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record CreateAlertCommand(string Title, string Description, DateTime Date, int TripId);