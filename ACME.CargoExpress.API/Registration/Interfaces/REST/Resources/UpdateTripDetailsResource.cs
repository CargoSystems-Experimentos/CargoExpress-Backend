namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateTripDetailsResource(
    string Name,
    string Type,
    decimal Weight,
    int DriverId,
    int VehicleId,
    int ClientId);
