namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidVehiclePlateException()
    : Exception("La placa del vehículo no puede estar vacía.");
