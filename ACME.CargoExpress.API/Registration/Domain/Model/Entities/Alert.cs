using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;

namespace ACME.CargoExpress.API.Registration.Domain.Model.Entities;

public class Alert
{
    public Alert()
    {
        Title = string.Empty;
        Type = string.Empty;
        Description = string.Empty;
        Date = DateTime.Now;
        Trip = new Trip();
    }

    public Alert(CreateAlertCommand command, Trip trip)
    {
        Title = command.Title;
        Type = command.Type;
        Description = command.Description;
        Date = command.Date;
        TripId = command.TripId;
        Trip = trip;
    }

    public int Id { get; set; }
    public string Title { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public int TripId { get; set; }
    public Trip Trip { get; set; }

    public void Update(UpdateAlertCommand command)
    {
        Title = command.Title;
        Type = command.Type;
        Description = command.Description;
        Date = command.Date;
    }
}