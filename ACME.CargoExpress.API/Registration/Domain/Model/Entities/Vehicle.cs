using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;

namespace ACME.CargoExpress.API.Registration.Domain.Model.Entities;

public class Vehicle
{
    public Vehicle()
    {
        Name = string.Empty;
        Model = string.Empty;
        Plate = string.Empty;
        TractorPlate = string.Empty;
        MaxLoad = 0;
        Volume = 0;
        State = "AVAILABLE";
        Entrepreneur = new Entrepreneur();
        Trips = new List<Trip>();
    }

    public Vehicle(string name, string model, string plate, string tractorPlate, decimal maxLoad, decimal volume, int entrepreneurId, Entrepreneur entrepreneur)
    {
        Name = name;
        Model = model;
        Plate = plate;
        TractorPlate = tractorPlate;
        MaxLoad = maxLoad;
        Volume = volume;
        State = "AVAILABLE";
        EntrepreneurId = entrepreneurId;
        Entrepreneur = entrepreneur;
        Trips = new List<Trip>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Model { get; set; }
    public string Plate { get; set; }
    public string TractorPlate { get; set; }
    public decimal MaxLoad { get; set; }
    public decimal Volume { get; set; }
    public string State { get; set; }

    public int EntrepreneurId { get; set; }
    public Entrepreneur Entrepreneur { get; set; }
    public ICollection<Trip> Trips { get; }
}