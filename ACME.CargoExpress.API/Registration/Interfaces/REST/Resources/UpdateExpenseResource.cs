namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateExpenseResource(decimal FuelAmount, string FuelDescription, decimal ViaticsAmount, string ViaticsDescription, decimal TollsAmount, string TollsDescription, int TripId);
