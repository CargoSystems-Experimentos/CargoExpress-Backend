namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateTripScheduleCommand(
    int TripId,
    string LoadLocation,
    DateTime LoadDate,
    string UnloadLocation,
    DateTime UnloadDate);
