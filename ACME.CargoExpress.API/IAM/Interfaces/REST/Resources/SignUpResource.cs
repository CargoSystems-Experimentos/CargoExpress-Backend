namespace ACME.CargoExpress.API.IAM.Interfaces.REST.Resources;

/**
 * <summary>
 *     The sign up resource.
 * </summary>
 * <remarks>
 *     Creates a user account together with a Client or Entrepreneur profile
 *     depending on the selected role.
 *     Role: false = CLIENT (0), true = ENTREPRENEUR (1)
 * </remarks>
 */
public record SignUpResource(string Username, string Password, string Phone, bool Role, SignUpProfileResource Profile);