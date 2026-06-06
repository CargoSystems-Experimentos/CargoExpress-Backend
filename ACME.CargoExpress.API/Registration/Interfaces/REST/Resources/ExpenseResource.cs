namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record ExpenseResource(int Id, decimal FuelAmount, string FuelDescription, decimal ViaticsAmount, string ViaticsDescription, decimal TollsAmount, string TollsDescription, int TripId, bool State);
