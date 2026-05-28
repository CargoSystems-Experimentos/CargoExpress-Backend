using System.Text.RegularExpressions;
using ACME.CargoExpress.API.IAM.Application.Internal.OutboundServices;
using ACME.CargoExpress.API.IAM.Domain.Exceptions;
using ACME.CargoExpress.API.IAM.Domain.Model.Commands;
using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.IAM.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

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
        var user = await userRepository.FindByUsernameAsync(command.Username);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

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
        if (string.IsNullOrWhiteSpace(command.Username) || !EmailRegex.IsMatch(command.Username))
            throw new InvalidUsernameException(command.Username);

        ValidatePassword(command.Password);

        if (userRepository.ExistsByUsername(command.Username))
            throw new DuplicateUsernameException(command.Username);

        var hashedPassword = hashingService.HashPassword(command.Password);
        var user = new Domain.Model.Aggregates.User(command.Username, hashedPassword);

        await userRepository.AddAsync(user);
        await unitOfWork.CompleteAsync();
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