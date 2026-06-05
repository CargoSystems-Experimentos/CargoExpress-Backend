namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateOngoingTripCommand(int OngoingTripId, float Latitude, float Longitude, int Speed, int Distance, int TripId);
