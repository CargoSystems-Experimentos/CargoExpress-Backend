namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record CreateOngoingTripResource(string State, float Latitude, float Longitude, int Speed, int Distance, int TripId);