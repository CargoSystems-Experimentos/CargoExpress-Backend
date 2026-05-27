namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record CreateExpenseCommand(int FuelAmount, string FuelDescription, int ViaticsAmount, string ViaticsDescription, int TollsAmount, string TollsDescription, int TripId);