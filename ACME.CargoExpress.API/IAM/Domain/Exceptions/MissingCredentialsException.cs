namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

public class MissingCredentialsException()
    : Exception("El correo electrónico y la contraseña no pueden estar vacíos.");
