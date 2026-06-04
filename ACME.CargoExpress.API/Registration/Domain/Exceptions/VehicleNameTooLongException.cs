namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class VehicleNameTooLongException()
    : Exception("El nombre del vehículo no puede exceder 60 caracteres.");
