namespace ACME.CargoExpress.API.IAM.Domain.Model.Commands;

/**
 * <summary>
 *     The sign up command
 * </summary>
 * <remarks>
 *     This command object includes the credentials, the phone and the role-specific
 *     profile data needed to create a user together with its Client or Entrepreneur profile.
 *     <see cref="Role"/> is kept as a raw string so the domain can validate it and report
 *     an invalid role with a meaningful Spanish error message.
 * </remarks>
 */
public record SignUpCommand(
    string Username,
    string Password,
    string Phone,
    string Role,
    string Name,
    string? Dni,
    string? Ruc);
