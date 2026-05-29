namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

public class DuplicateUserPhoneException(string phone)
    : Exception($"Ya existe un usuario registrado con el teléfono '{phone}'.")
{
    public string Phone { get; } = phone;
}
