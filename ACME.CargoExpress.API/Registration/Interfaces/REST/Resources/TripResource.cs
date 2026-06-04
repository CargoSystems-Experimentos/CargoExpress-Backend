namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record TripResource(
    int Id,
    string Name,
    string Type,
    decimal Weight,
    string LoadLocation,
    DateTime LoadDate,
    string UnloadLocation,
    DateTime UnloadDate,
    int DriverId,
    int VehicleId,
    int ClientId,
    int EntrepreneurId);