namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateExpenseCommand(int ExpenseId, decimal FuelAmount, string FuelDescription, decimal ViaticsAmount, string ViaticsDescription, decimal TollsAmount, string TollsDescription, int TripId);
