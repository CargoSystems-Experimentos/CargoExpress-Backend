using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;

namespace ACME.CargoExpress.API.User.Domain.Model.Aggregates;

public class Entrepreneur
{
    public Entrepreneur()
    {
        Name = string.Empty;
        Ruc = string.Empty;
        Address = string.Empty;
        User = new IAM.Domain.Model.Aggregates.User();
        Trips = new List<Trip>();
    }

    public Entrepreneur(string name, string ruc, string address, int userId, IAM.Domain.Model.Aggregates.User user)
    {
        Name = name;
        Ruc = ruc;
        Address = address;
        UserId = userId;
        User = user;
        Trips = new List<Trip>();
    }

    public Entrepreneur(CreateEntrepreneurCommand command, IAM.Domain.Model.Aggregates.User user)
    {
        Name = command.Name;
        Ruc = command.Ruc;
        Address = command.Address;
        UserId = command.UserId;
        User = user;
        Trips = new List<Trip>();
    }

    public void Update(UpdateEntrepreneurCommand command)
    {
        Name = command.Name;
        Ruc = command.Ruc;
        Address = command.Address;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Ruc { get; set; }
    public string Address { get; set; }
    public IAM.Domain.Model.Aggregates.User User { get; set; }
    public int UserId { get; set; }

    public ICollection<Trip> Trips { get; }
    public ICollection<Vehicle> Vehicles { get; }
    public ICollection<Driver> Drivers { get; }
}