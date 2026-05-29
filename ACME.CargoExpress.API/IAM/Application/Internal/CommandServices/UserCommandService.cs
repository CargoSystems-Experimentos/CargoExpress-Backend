using System.Text.RegularExpressions;
using ACME.CargoExpress.API.IAM.Application.Internal.OutboundServices;
using ACME.CargoExpress.API.IAM.Domain.Exceptions;
using ACME.CargoExpress.API.IAM.Domain.Model.Commands;
using ACME.CargoExpress.API.IAM.Domain.Model.ValueObjects;
using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.IAM.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.IAM.Application.Internal.CommandServices;

/**
 * <summary>
 *     The user command service
 * </summary>
 * <remarks>
 *     This class is used to handle user commands
 * </remarks>
 */
public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IClientCommandService clientCommandService,
    IEntrepreneurCommandService entrepreneurCommandService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<(Domain.Model.Aggregates.User user, string token)> Handle(SignInCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Username) || string.IsNullOrWhiteSpace(command.Password))
            throw new MissingCredentialsException();

        var user = await userRepository.FindByUsernameAsync(command.Username);

        // Use a single generic message so we never reveal whether the email exists.
        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new InvalidCredentialsException();

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task Handle(SignUpCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Role) || !Enum.IsDefined(typeof(ERole), command.Role))
            throw new InvalidRoleException(command.Role ?? string.Empty);

        var role = Enum.Parse<ERole>(command.Role);
        ValidateProfile(role, command);

        if (string.IsNullOrWhiteSpace(command.Username) || !EmailRegex.IsMatch(command.Username))
            throw new InvalidUsernameException(command.Username);

        ValidatePassword(command.Password);
        ValidatePhone(command.Phone);

        if (userRepository.ExistsByUsername(command.Username))
            throw new DuplicateUsernameException(command.Username);

        if (await userRepository.FindByPhoneAsync(command.Phone) is not null)
            throw new DuplicateUserPhoneException(command.Phone);

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new Domain.Model.Aggregates.User(command.Username, hashedPassword, command.Phone);

        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        // Create the role-specific profile. If it fails validation, roll back the user
        // so we never leave an account without a profile.
        try
        {
            switch (role)
            {
                case ERole.CLIENT:
                    await clientCommandService.Handle(
                        new CreateClientCommand(command.Name, command.Dni ?? string.Empty, command.BirthDate ?? DateTime.MinValue, user.Id));
                    break;
                case ERole.ENTREPRENEUR:
                    await entrepreneurCommandService.Handle(
                        new CreateEntrepreneurCommand(command.Name, command.Ruc ?? string.Empty, command.Address ?? string.Empty, user.Id));
                    break;
                default:
                    throw new InvalidRoleException(command.Role);
            }
        }
        catch
        {
            userRepository.Remove(user);
            await unitOfWork.CompleteAsync();
            throw;
        }
    }

    private static void ValidateProfile(ERole role, SignUpCommand command)
    {
        switch (role)
        {
            // A Client must register only name, DNI and BirthDate; a RUC or Address is not allowed.
            case ERole.CLIENT when !string.IsNullOrWhiteSpace(command.Ruc):
                throw new InvalidProfileException(
                    "Un cliente solo debe registrar nombre, DNI y fecha de nacimiento; no debe incluir RUC.");
            case ERole.CLIENT when !string.IsNullOrWhiteSpace(command.Address):
                throw new InvalidProfileException(
                    "Un cliente solo debe registrar nombre, DNI y fecha de nacimiento; no debe incluir dirección.");
            // An Entrepreneur must register only name, RUC and Address; a DNI is not allowed.
            case ERole.ENTREPRENEUR when !string.IsNullOrWhiteSpace(command.Dni):
                throw new InvalidProfileException(
                    "Un emprendedor solo debe registrar nombre, RUC y dirección; no debe incluir DNI.");
        }
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new InvalidUserPhoneException("El teléfono es obligatorio.");

        if (phone.Length != 9)
            throw new InvalidUserPhoneException("El teléfono debe tener exactamente 9 caracteres.");

        if (!phone.All(char.IsDigit))
            throw new InvalidUserPhoneException("El teléfono solo debe contener números.");
    }

    private static void ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            throw new InvalidPasswordException("La contraseña debe tener al menos 8 caracteres.");

        if (!password.Any(char.IsUpper))
            throw new InvalidPasswordException("La contraseña debe contener al menos una letra mayúscula.");

        if (!password.Any(char.IsDigit))
            throw new InvalidPasswordException("La contraseña debe contener al menos un número.");

        if (!password.Any(c => !char.IsLetterOrDigit(c)))
            throw new InvalidPasswordException("La contraseña debe contener al menos un carácter especial.");
    }
}