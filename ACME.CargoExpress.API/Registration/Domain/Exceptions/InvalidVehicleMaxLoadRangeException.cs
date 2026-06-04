namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidVehicleMaxLoadRangeException()
    : Exception("La carga máxima del vehículo debe tener como máximo 2 decimales y hasta 10 digitos enteros");