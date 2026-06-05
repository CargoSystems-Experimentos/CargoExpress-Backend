namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record OngoingTripResource(int Id, float Latitude, float Longitude, int Speed, int Distance, int TripId);