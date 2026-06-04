namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidDriverDniLengthException()
    : Exception("El DNI debe tener exactamente 8 caracteres.");
