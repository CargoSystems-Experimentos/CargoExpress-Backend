namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class ClientNotFoundException(int clientId)
    : Exception($"No se encontró un cliente con el id '{clientId}'.")
{
    public int ClientId { get; } = clientId;
}
