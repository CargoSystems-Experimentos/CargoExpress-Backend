namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateTripCommand(
    int TripId,
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