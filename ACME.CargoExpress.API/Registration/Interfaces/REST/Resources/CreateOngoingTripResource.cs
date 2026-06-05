namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record CreateOngoingTripResource(float Latitude, float Longitude, int Speed, int Distance, int TripId);