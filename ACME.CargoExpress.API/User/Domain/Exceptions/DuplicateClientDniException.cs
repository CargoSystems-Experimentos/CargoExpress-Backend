namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class DuplicateClientDniException(string dni)
    : Exception($"Ya existe un cliente registrado con el DNI '{dni}'.")
{
    public string Dni { get; } = dni;
}
