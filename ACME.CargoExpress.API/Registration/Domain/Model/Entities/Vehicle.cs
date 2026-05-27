using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;

namespace ACME.CargoExpress.API.Registration.Domain.Model.Entities;

public class Vehicle
{
    public Vehicle()
    {
        Model = string.Empty;
        Plate = string.Empty;
        TractorPlate = string.Empty;
        MaxLoad = 0;
        Volume = 0;
        Entrepreneur = new Entrepreneur();
        Trips = new List<Trip>();
    }

    public Vehicle(string model, string plate, string tractorPlate, float maxLoad, float volume, int entrepreneurId, Entrepreneur entrepreneur)
    {
        Model = model;
        Plate = plate;
        TractorPlate = tractorPlate;
        MaxLoad = maxLoad;
        Volume = volume;
        EntrepreneurId = entrepreneurId;
        Entrepreneur = entrepreneur;
        Trips = new List<Trip>();
    }
    
    public int Id { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
    public string TractorPlate { get; set; }
    public float MaxLoad { get; set; }
    public float Volume { get; set; }

    public int EntrepreneurId { get; set; }
    public Entrepreneur Entrepreneur { get; set; }
    public ICollection<Trip> Trips { get; }
}