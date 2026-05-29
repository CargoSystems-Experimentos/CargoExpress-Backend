namespace ACME.CargoExpress.API.IAM.Domain.Exceptions;

/**
 * <summary>
 *     Thrown when the profile data does not match the selected role: a Client must
 *     provide only name and DNI, while an Entrepreneur must provide only name and RUC.
 * </summary>
 */
public class InvalidProfileException(string message) : Exception(message);
