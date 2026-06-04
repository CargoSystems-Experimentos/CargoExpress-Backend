namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidDriverPhoneLengthException()
    : Exception("El número de teléfono debe tener exactamente 9 caracteres.");
