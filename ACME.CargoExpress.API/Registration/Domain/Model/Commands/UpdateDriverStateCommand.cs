namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateDriverStateCommand(int DriverId, string State);
