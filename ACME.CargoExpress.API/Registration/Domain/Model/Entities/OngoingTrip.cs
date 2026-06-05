using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;

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
    }

    public OngoingTrip(float latitude, float longitude, int speed, int distance, int tripId, Trip trip)
    {
        Latitude = latitude;
        Longitude = longitude;
        Speed = speed;
        Distance = distance;
        TripId = tripId;
        Trip = trip;
    }

    public OngoingTrip(CreateOngoingTripCommand command, Trip trip)
    {
        Latitude = command.Latitude;
        Longitude = command.Longitude;
        Speed = command.Speed;
        Distance = command.Distance;
        Trip = trip;
    }

    public int Id { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
    public int Speed { get; set; }
    public int Distance { get; set; }

    public int TripId { get; set; }
    public Trip Trip { get; }
}