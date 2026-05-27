namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record UpdateExpenseResource(int FuelAmount, string FuelDescription, int ViaticsAmount, string ViaticsDescription, int TollsAmount, string TollsDescription, int TripId);