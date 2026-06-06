namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateTripScheduleResource(
    string LoadLocation,
    DateTime LoadDate,
    string UnloadLocation,
    DateTime UnloadDate);
