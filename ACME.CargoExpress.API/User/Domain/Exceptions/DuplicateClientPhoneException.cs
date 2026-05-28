namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class DuplicateClientPhoneException(string phone)
    : Exception($"Ya existe un cliente registrado con el teléfono '{phone}'.")
{
    public string Phone { get; } = phone;
}
