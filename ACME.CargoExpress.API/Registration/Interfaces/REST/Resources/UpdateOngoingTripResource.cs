namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateOngoingTripResource(float Latitude, float Longitude, int Speed, int Distance, int TripId);