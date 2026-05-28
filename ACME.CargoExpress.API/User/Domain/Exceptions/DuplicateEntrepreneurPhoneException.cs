namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class DuplicateEntrepreneurPhoneException(string phone)
    : Exception($"Ya existe un emprendedor registrado con el teléfono '{phone}'.")
{
    public string Phone { get; } = phone;
}
