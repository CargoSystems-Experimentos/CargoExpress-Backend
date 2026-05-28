using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;

namespace ACME.CargoExpress.API.User.Domain.Model.Aggregates;

public class Entrepreneur
{
    public Entrepreneur()
    {
        Name = string.Empty;
        Phone = string.Empty;
        Ruc = string.Empty;
        User = new IAM.Domain.Model.Aggregates.User();
        Trips = new List<Trip>();
    }

    public Entrepreneur(string name, string phone, string ruc, int userId, IAM.Domain.Model.Aggregates.User user)
    {
        Name = name;
        Phone = phone;
        Ruc = ruc;
        UserId = userId;
        User = user;
        Trips = new List<Trip>();
    }

    public Entrepreneur(CreateEntrepreneurCommand command, IAM.Domain.Model.Aggregates.User user)
    {
        Name = command.Name;
        Phone = command.Phone;
        Ruc = command.Ruc;
        UserId = command.UserId;
        User = user;
        Trips = new List<Trip>();
    }

    public void Update(UpdateEntrepreneurCommand command)
    {
        Name = command.Name;
        Phone = command.Phone;
        Ruc = command.Ruc;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public string Ruc { get; set; }
    public IAM.Domain.Model.Aggregates.User User { get; set; }
    public int UserId { get; set; }

    public ICollection<Trip> Trips { get; }
    public ICollection<Vehicle> Vehicles { get; }
    public ICollection<Driver> Drivers { get; }
}