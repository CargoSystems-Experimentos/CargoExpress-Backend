using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;

namespace ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;

public class Trip
{
    public Trip()
    {
        Name = string.Empty;
        Type = string.Empty;
        Weight = 0;
        LoadLocation = string.Empty;
        LoadDate = DateTime.MinValue;
        UnloadLocation = string.Empty;
        UnloadDate = DateTime.MinValue;
        Driver = new Driver();
        Vehicle = new Vehicle();
        Client = new Client();
        Entrepreneur = new Entrepreneur();
    }

    public Trip(string name, string type, decimal weight, string loadLocation, DateTime loadDate, string unloadLocation, DateTime unloadDate,
        int driverId, int vehicleId, int clientId, int entrepreneurId, Driver driver, Vehicle vehicle, Client client, Entrepreneur entrepreneur)
    {
        Name = name;
        Type = type;
        Weight = weight;
        LoadLocation = loadLocation;
        LoadDate = loadDate;
        UnloadLocation = unloadLocation;
        UnloadDate = unloadDate;
        DriverId = driverId;
        VehicleId = vehicleId;
        ClientId = clientId;
        EntrepreneurId = entrepreneurId;
        Driver = driver;
        Vehicle = vehicle;
        Client = client;
        Entrepreneur = entrepreneur;
    }

    public Trip(CreateTripCommand command, Driver driver, Vehicle vehicle, Client client, Entrepreneur entrepreneur)
    {
        Name = command.Name;
        Type = command.Type;
        Weight = command.Weight;
        LoadLocation = command.LoadLocation;
        LoadDate = command.LoadDate;
        UnloadLocation = command.UnloadLocation;
        UnloadDate = command.UnloadDate;
        DriverId = command.DriverId;
        VehicleId = command.VehicleId;
        ClientId = command.ClientId;
        EntrepreneurId = command.EntrepreneurId;
        Driver = driver;
        Vehicle = vehicle;
        Client = client;
        Entrepreneur = entrepreneur;
        State = "AWAITING";
    }
    
    public int Id { get; set; }
    public string Name { get; set; }
    public string State { get; set; } = "AWAITING";
    public string Type { get; set; }
    public decimal Weight { get; set; }
    public string LoadLocation { get; set; }
    public DateTime LoadDate { get; set; }
    public string UnloadLocation { get; set; }
    public DateTime UnloadDate { get; set; }

    public Driver Driver { get; set; }
    public Vehicle Vehicle { get; set; }
    public int DriverId { get; set; }
    public int VehicleId { get; set; }
    public int ClientId { get; set; }
    public int EntrepreneurId { get; set; }
    public Client Client { get; set; }
    public Entrepreneur Entrepreneur { get; set; }
    
    public Expense Expense { get; set; }
    public ICollection<Alert> Alerts { get; set; }
    public OngoingTrip OngoingTrip { get; set; }
}