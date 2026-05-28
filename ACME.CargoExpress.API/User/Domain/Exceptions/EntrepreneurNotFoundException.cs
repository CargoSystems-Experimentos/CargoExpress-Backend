namespace ACME.CargoExpress.API.User.Domain.Exceptions;

public class EntrepreneurNotFoundException(int entrepreneurId)
    : Exception($"No se encontró un emprendedor con el id '{entrepreneurId}'.")
{
    public int EntrepreneurId { get; } = entrepreneurId;
}
