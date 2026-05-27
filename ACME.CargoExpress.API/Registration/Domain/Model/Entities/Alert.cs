using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;

namespace ACME.CargoExpress.API.Registration.Domain.Model.Entities;

public class Alert
{
    public Alert()
    {
        Title= string.Empty;
        Description = string.Empty;
        Date = DateTime.Now;
        OngoingTrip = new OngoingTrip();
    }
    
    public Alert(string title, string description, DateTime date, int ongoingTripId, OngoingTrip ongoingTrip)
    {
        Title = title;
        Description = description;
        Date = date;
        OngoingTripId = ongoingTripId;
        OngoingTrip = ongoingTrip;
    }

    public Alert(CreateAlertCommand command, OngoingTrip ongoingTrip)
    {
        Title = command.Title;
        Description = command.Description;
        Date = command.Date;
        OngoingTrip = ongoingTrip;
    }
    
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public int OngoingTripId { get; set; }
    public OngoingTrip OngoingTrip { get; }
}