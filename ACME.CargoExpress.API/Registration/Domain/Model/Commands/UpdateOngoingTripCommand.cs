namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

// State is a plain string now
public record UpdateOngoingTripCommand(int OngoingTripId, string State, float Latitude, float Longitude, int Speed, int Distance, int TripId);
