namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

public class DuplicateUsernameException(string username)
    : Exception($"Un usuario con el nombre de usuario '{username}' ya existe.")
{
    public string Username { get; } = username;
}
