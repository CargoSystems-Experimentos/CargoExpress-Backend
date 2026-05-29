namespace ACME.CargoExpress.API.IAM.Interfaces.REST.Resources;

/**
 * <summary>
 *     The profile section of a sign up request.
 * </summary>
 * <remarks>
 *     <see cref="Dni"/> is used when the role is CLIENT and <see cref="Ruc"/>
 *     is used when the role is ENTREPRENEUR.
 * </remarks>
 */
public record SignUpProfileResource(string Name, string? Dni, DateTime? BirthDate, string? Ruc, string? Address);
