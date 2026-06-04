namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidVehicleVolumeRangeException()
    : Exception("El volumen del vehículo debe tener como máximo 2 decimales y hasta 10 digitos enteros");