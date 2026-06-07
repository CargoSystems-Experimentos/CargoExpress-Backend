namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

public class InactiveUserException()
    : Exception("La cuenta de usuario está desactivada. Contacte al administrador para reactivarla.");
