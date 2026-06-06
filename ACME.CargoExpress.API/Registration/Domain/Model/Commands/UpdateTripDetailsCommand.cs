namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateTripDetailsCommand(
    int TripId,
    string Name,
    string Type,
    decimal Weight,
    int DriverId,
    int VehicleId,
    int ClientId);
