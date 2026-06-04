namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidVehicleStateException()
    : Exception("El estado del vehículo debe ser AVAILABLE, UNAVAILABLE o INACTIVE.");
