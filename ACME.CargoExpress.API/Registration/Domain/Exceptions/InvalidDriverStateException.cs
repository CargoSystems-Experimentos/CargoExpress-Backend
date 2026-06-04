namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class InvalidDriverStateException()
    : Exception("El estado del conductor debe ser AVAILABLE, UNAVAILABLE o INACTIVE.");
