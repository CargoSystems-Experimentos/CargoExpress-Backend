using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.IAM.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;

namespace ACME.CargoExpress.API.User.Domain.Model.Aggregates;

public class Client
{
    public Client()
    {
        Name = string.Empty;
        Dni = string.Empty;
        User = new IAM.Domain.Model.Aggregates.User();
        Trips = new List<Trip>();
    }

    public Client(string name, string dni, int userId, IAM.Domain.Model.Aggregates.User user)
    {
        Name = name;
        Dni = dni;
        UserId = userId;
        User = user;
        Trips = new List<Trip>();
    }

    public Client(CreateClientCommand command, IAM.Domain.Model.Aggregates.User user)
    {
        Name = command.Name;
        Dni = command.Dni;
        UserId = command.UserId;
        User = user;
        Trips = new List<Trip>();
    }

    public void Update(UpdateClientCommand command)
    {
        Name = command.Name;
        Dni = command.Dni;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Dni { get; set; }
    public IAM.Domain.Model.Aggregates.User User { get; set; }
    public int UserId { get; set; }
    
    public ICollection<Trip> Trips { get; }
}