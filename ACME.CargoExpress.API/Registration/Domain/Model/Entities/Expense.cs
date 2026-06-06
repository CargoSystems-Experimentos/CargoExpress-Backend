using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;

namespace ACME.CargoExpress.API.Registration.Domain.Model.Entities;

public class Expense
{
    public Expense()
    {
        FuelAmount = 0;
        FuelDescription = string.Empty;
        ViaticsAmount = 0;
        ViaticsDescription = string.Empty;
        TollsAmount = 0;
        TollsDescription = string.Empty;
        State = true;
        Trip = new Trip();
    }

    public Expense(decimal fuelAmount, string fuelDescription, decimal viaticsAmount, string viaticsDescription, decimal tollsAmount, string tollsDescription, int tripId, Trip trip)
    {
        FuelAmount = fuelAmount;
        FuelDescription = fuelDescription;
        ViaticsAmount = viaticsAmount;
        ViaticsDescription = viaticsDescription;
        TollsAmount = tollsAmount;
        TollsDescription = tollsDescription;
        TripId = tripId;
        State = true;
        Trip = trip;
    }

    public Expense(CreateExpenseCommand command, Trip trip)
    {
        FuelAmount = command.FuelAmount;
        FuelDescription = command.FuelDescription;
        ViaticsAmount = command.ViaticsAmount;
        ViaticsDescription = command.ViaticsDescription;
        TollsAmount = command.TollsAmount;
        TollsDescription = command.TollsDescription;
        State = true;
        Trip = trip;
    }

    public int Id { get; set; }
    public decimal FuelAmount { get; set; }
    public string FuelDescription { get; set; }
    public decimal ViaticsAmount { get; set; }
    public string ViaticsDescription { get; set; }
    public decimal TollsAmount { get; set; }
    public string TollsDescription { get; set; }
    public bool State { get; set; }

    public int TripId { get; set; }

    public Trip Trip { get; }

}