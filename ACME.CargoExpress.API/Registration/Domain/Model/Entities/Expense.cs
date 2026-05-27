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
        Trip = new Trip();
    }

    public Expense(int fuelAmount, string fuelDescription, int viaticsAmount, string viaticsDescription, int tollsAmount, string tollsDescription, int tripId, Trip trip)
    {
        FuelAmount = fuelAmount;
        FuelDescription = fuelDescription;
        ViaticsAmount = viaticsAmount;
        ViaticsDescription = viaticsDescription;
        TollsAmount = tollsAmount;
        TollsDescription = tollsDescription;
        TripId = tripId;
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
        Trip = trip;
    }
    
    public int Id { get; set; }
    public int FuelAmount { get; set; }
    public string FuelDescription { get; set; }
    public int ViaticsAmount { get; set; }
    public string ViaticsDescription { get; set; }
    public int TollsAmount { get; set; }
    public string TollsDescription { get; set; }
     
    public int TripId { get; set; }
    
    public Trip Trip { get; }

}