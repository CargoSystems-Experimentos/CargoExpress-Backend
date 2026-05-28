namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

public class InvalidUsernameException(string username)
    : Exception($"El nombre de usuario '{username}' no es una dirección de correo válida. Formato esperado: usuario@dominio.com.")
{
    public string Username { get; } = username;
}