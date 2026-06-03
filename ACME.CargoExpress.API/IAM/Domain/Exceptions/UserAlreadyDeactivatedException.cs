namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

public class UserAlreadyDeactivatedException(int userId)
    : Exception($"La cuenta del usuario {userId} ya está desactivada.");
