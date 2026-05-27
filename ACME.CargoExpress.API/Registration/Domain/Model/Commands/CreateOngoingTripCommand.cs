namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

// State is a plain string now (e.g. "PENDIENTE","PROGRESO","COMPLETADO","CANCELADO")
public record CreateOngoingTripCommand(string State, float Latitude, float Longitude, int Speed, int Distance, int TripId);
