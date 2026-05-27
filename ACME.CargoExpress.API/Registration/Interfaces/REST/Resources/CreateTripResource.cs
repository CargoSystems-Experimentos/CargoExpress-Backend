namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record CreateTripResource(
    string Name,
    string Type,
    int Weight,
    string LoadLocation,
    DateTime LoadDate,
    string UnloadLocation,
    DateTime UnloadDate,
    int DriverId,
    int VehicleId,
    int ClientId,
    int EntrepreneurId,
    string EvidenceImg);