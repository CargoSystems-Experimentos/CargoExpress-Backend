using System.Net.Mime;
using ACME.CargoExpress.API.IAM.Domain.Exceptions;
using ACME.CargoExpress.API.IAM.Domain.Services;
using ACME.CargoExpress.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using ACME.CargoExpress.API.IAM.Interfaces.REST.Resources;
using ACME.CargoExpress.API.IAM.Interfaces.REST.Transform;
using ACME.CargoExpress.API.User.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.IAM.Interfaces.REST;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
{
    /**
     * <summary>
     *     Sign in endpoint. It allows to authenticate a user
     * </summary>
     * <param name="signInResource">The sign in resource containing username and password.</param>
     * <returns>The authenticated user resource, including a JWT token</returns>
     */
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
        var authenticatedUser = await userCommandService.Handle(signInCommand);
        var resource =
            AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                authenticatedUser.token);
        return Ok(resource);
    }

    /**
     * <summary>
     *     Sign up endpoint. It creates a new user account together with its
     *     role-specific profile (Client or Entrepreneur).
     * </summary>
     * <remarks>
     *     The request body must include the role (CLIENT or ENTREPRENEUR) and a
     *     profile section with the role-specific fields.
     *
     *     Registration as Client:
     *
     *         {
     *           "username": "usuario@dominio.com",
     *           "password": "Password1!",
     *           "phone": "987654321",
     *           "role": "CLIENT",
     *           "profile": { "name": "Juan Gomez", "dni": "12345678" }
     *         }
     *
     *     Registration as Entrepreneur:
     *
     *         {
     *           "username": "empresa@dominio.com",
     *           "password": "Password1!",
     *           "phone": "987654322",
     *           "role": "ENTREPRENEUR",
     *           "profile": { "name": "Transportes SAC", "ruc": "20123456789" }
     *         }
     * </remarks>
     * <param name="signUpResource">The sign up resource containing the credentials, phone, role and profile.</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
        try
        {
            await userCommandService.Handle(signUpCommand);
            return Ok(new { message = "Usuario creado exitosamente." });
        }
        // Invalid input (400 Bad Request)
        catch (InvalidRoleException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidProfileException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidUsernameException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidPasswordException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidUserPhoneException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidClientNameException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidClientDniException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidEntrepreneurNameException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidEntrepreneurRucException e)
        {
            return BadRequest(new { message = e.Message });
        }
        // Conflicting unique values (409 Conflict)
        catch (DuplicateUsernameException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (DuplicateUserPhoneException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (DuplicateClientDniException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (DuplicateEntrepreneurNameException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (DuplicateEntrepreneurRucException e)
        {
            return Conflict(new { message = e.Message });
        }
    }
}