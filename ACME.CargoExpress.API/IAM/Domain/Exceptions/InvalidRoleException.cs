namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

public class InvalidRoleException(string role)
    : Exception($"El rol '{role}' no es válido. Los roles permitidos son CLIENT y ENTREPRENEUR.")
{
    public string Role { get; } = role;
}
