using System.Text.RegularExpressions;
using ACME.CargoExpress.API.IAM.Application.Internal.OutboundServices;
using ACME.CargoExpress.API.IAM.Domain.Exceptions;
using ACME.CargoExpress.API.IAM.Domain.Model.Commands;
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

        // Deactivated accounts are not allowed to sign in even with valid credentials.
        if (!user.State)
            throw new InactiveUserException();

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
        ValidateProfile(command.Role, command);

        if (string.IsNullOrWhiteSpace(command.Username) || !EmailRegex.IsMatch(command.Username))
            throw new InvalidUsernameException(command.Username);

        ValidatePassword(command.Password);
        ValidatePhone(command.Phone);

        if (userRepository.ExistsByUsername(command.Username))
            throw new DuplicateUsernameException(command.Username);

        if (await userRepository.FindByPhoneAsync(command.Phone) is not null)
            throw new DuplicateUserPhoneException(command.Phone);

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new Domain.Model.Aggregates.User(command.Username, hashedPassword, command.Phone, command.Role);

        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();

        // Create the role-specific profile. If it fails validation, roll back the user
        // so we never leave an account without a profile.
        try
        {
            if (command.Role == false) // CLIENT
            {
                await clientCommandService.Handle(
                    new CreateClientCommand(command.Name, command.Dni ?? string.Empty, command.BirthDate ?? DateTime.MinValue, user.Id));
            }
            else // ENTREPRENEUR
            {
                await entrepreneurCommandService.Handle(
                    new CreateEntrepreneurCommand(command.Name, command.Ruc ?? string.Empty, command.Address ?? string.Empty, user.Id));
            }
        }
        catch
        {
            userRepository.Remove(user);
            await unitOfWork.CompleteAsync();
            throw;
        }
    }

    public async Task<Domain.Model.Aggregates.User> Handle(UpdateUserStateCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId)
            ?? throw new UserNotFoundException(command.UserId);

        if (user.State == command.State)
        {
            if (command.State)
                throw new UserAlreadyActiveException(command.UserId);
            throw new UserAlreadyDeactivatedException(command.UserId);
        }

        user.UpdateState(command.State);
        userRepository.Update(user);
        await unitOfWork.CompleteAsync();

        return user;
    }

    private static void ValidateProfile(bool role, SignUpCommand command)
    {
        if (role == false) // CLIENT
        {
            // A Client must register only name, DNI and BirthDate; a RUC or Address is not allowed.
            if (!string.IsNullOrWhiteSpace(command.Ruc))
                throw new InvalidProfileException(
                    "Un cliente solo debe registrar nombre, DNI y fecha de nacimiento; no debe incluir RUC.");
            if (!string.IsNullOrWhiteSpace(command.Address))
                throw new InvalidProfileException(
                    "Un cliente solo debe registrar nombre, DNI y fecha de nacimiento; no debe incluir dirección.");
        }
        else // ENTREPRENEUR
        {
            // An Entrepreneur must register only name, RUC and Address; a DNI is not allowed.
            if (!string.IsNullOrWhiteSpace(command.Dni))
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