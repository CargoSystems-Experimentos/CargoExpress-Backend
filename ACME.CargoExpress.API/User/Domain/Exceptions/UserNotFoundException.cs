namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class UserNotFoundException(int userId)
    : Exception($"No se encontró un usuario con el id '{userId}'.")
{
    public int UserId { get; } = userId;
}
