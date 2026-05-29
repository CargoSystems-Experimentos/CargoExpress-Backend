namespace ACME.CargoExpress.API.IAM.Interfaces.REST.Resources;

/**
 * <summary>
 *     The sign up resource.
 * </summary>
 * <remarks>
 *     Creates a user account together with a Client or Entrepreneur profile
 *     depending on the selected <see cref="Role"/>. The role is received as a raw
 *     string so that invalid values (e.g. ADMIN or empty) can be validated in the
 *     domain and reported with a meaningful Spanish error message instead of failing
 *     during JSON deserialization.
 * </remarks>
 */
public record SignUpResource(string Username, string Password, string Phone, string Role, SignUpProfileResource Profile);
