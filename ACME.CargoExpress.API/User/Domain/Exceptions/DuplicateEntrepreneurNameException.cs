namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class DuplicateEntrepreneurNameException(string name)
    : Exception($"Ya existe un emprendedor registrado con el nombre '{name}'.")
{
    public string Name { get; } = name;
}
