namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidVehicleVolumeException()
    : Exception("El volumen del vehículo no puede estar vacío.");
