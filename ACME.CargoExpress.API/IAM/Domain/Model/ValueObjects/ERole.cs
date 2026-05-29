using System.Text.Json.Serialization;

namespace ACME.CargoExpress.API.IAM.Domain.Model.ValueObjects;

/**
 * <summary>
 *     The role a user chooses when signing up.
 * </summary>
 * <remarks>
 *     Determines whether a Client or an Entrepreneur profile is created
 *     alongside the user account.
 * </remarks>
 */
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ERole
{
    CLIENT,
    ENTREPRENEUR
}
