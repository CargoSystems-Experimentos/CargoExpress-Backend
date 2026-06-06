namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record CreateExpenseCommand(decimal FuelAmount, string FuelDescription, decimal ViaticsAmount, string ViaticsDescription, decimal TollsAmount, string TollsDescription, int TripId);
