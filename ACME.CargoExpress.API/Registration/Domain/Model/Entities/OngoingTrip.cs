using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
// removed enum value object, using string for State now

namespace ACME.CargoExpress.API.Registration.Domain.Model.Entities;

public class OngoingTrip
{
    public OngoingTrip()
    {
        Latitude = 0;
        Longitude = 0;
        Speed = 0;
        Distance = 0;
        Trip = new Trip();
        State = "PENDIENTE";
    }

    public OngoingTrip(float latitude, float longitude, int speed, int distance, int tripId, Trip trip)
    {
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        Distance = distance;
        TripId = tripId;
        Trip = trip;
        State = "PENDIENTE";
    }

    public OngoingTrip(CreateOngoingTripCommand command, Trip trip)
    {
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        Speed = command.Speed;
        Distance = command.Distance;
        Trip = trip;
        var s = command.State?.ToString();
        State = string.IsNullOrWhiteSpace(s) ? "PENDIENTE" : s!;
    }

    public int Id { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public int Speed { get; set; }
    public int Distance { get; set; }

    // relación con trip original
    public int TripId { get; set; }
    public Trip Trip { get; }

    // nuevo estado (string)
    public string State { get; set; }
}