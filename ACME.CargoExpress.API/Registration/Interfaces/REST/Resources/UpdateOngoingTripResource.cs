namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateOngoingTripResource(string State, float Latitude, float Longitude, int Speed, int Distance, int TripId);