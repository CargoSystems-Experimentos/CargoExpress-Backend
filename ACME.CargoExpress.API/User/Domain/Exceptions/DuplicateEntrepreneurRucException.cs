namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class DuplicateEntrepreneurRucException(string ruc)
    : Exception($"Ya existe un emprendedor registrado con el RUC '{ruc}'.")
{
    public string Ruc { get; } = ruc;
}
