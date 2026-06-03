using System.Net.Mime;
using ACME.CargoExpress.API.IAM.Domain.Exceptions;
using ACME.CargoExpress.API.IAM.Domain.Model.Commands;
using ACME.CargoExpress.API.IAM.Domain.Model.Queries;
using ACME.CargoExpress.API.IAM.Domain.Services;
using ACME.CargoExpress.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using ACME.CargoExpress.API.IAM.Interfaces.REST.Resources;
using Microsoft.AspNetCore.Mvc;
using UserResourceFromEntityAssembler = ACME.CargoExpress.API.IAM.Interfaces.REST.Transform.UserResourceFromEntityAssembler;

namespace ACME.CargoExpress.API.IAM.Interfaces.REST;

/**
 * <summary>
 *     The users controller
 * </summary>
 * <remarks>
 *     This class is used to handle user requests
 * </remarks>
 */
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(IUserQueryService userQueryService, IUserCommandService userCommandService) : ControllerBase
{
    /**
     * <summary>
     *     Get user by id endpoint. It allows to get a user by id
     * </summary>
     * <param name="userId">The user id</param>
     * <returns>The user resource</returns>
     */
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var user = await userQueryService.Handle(getUserByIdQuery);
        var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
        return Ok(userResource);
    }

    /**
     * <summary>
     *     Get all users endpoint. It allows to get all users
     * </summary>
     * <returns>The user resources</returns>
     */
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var getAllUsersQuery = new GetAllUsersQuery();
        var users = await userQueryService.Handle(getAllUsersQuery);
        var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(userResources);
    }

    [HttpGet("{userId}/role")]
    public async Task<IActionResult> GetUserRoleById([FromRoute] int userId)
    {
        var user = await userQueryService.Handle(new GetUserByIdQuery(userId));
        if (user == null) return NotFound();
        return Ok(new { role = user.Role });
    }

    /**
     * <summary>
     *     Update user state endpoint. Activates or deactivates a user account.
     * </summary>
     * <param name="userId">The user id</param>
     * <param name="resource">The resource containing the new state (true = ACTIVE, false = INACTIVE)</param>
     * <returns>The updated user resource</returns>
     */
    [HttpPut("{userId}/state")]
    public async Task<IActionResult> UpdateUserState([FromRoute] int userId, [FromBody] UpdateUserStateResource resource)
    {
        if (resource.State is null)
            return BadRequest(new { message = "El campo 'state' es obligatorio y debe ser un valor booleano (true o false)." });

        try
        {
            var command = new UpdateUserStateCommand(userId, resource.State.Value);
            var user = await userCommandService.Handle(command);
            return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
        }
        catch (UserNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
        catch (UserAlreadyActiveException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (UserAlreadyDeactivatedException e)
        {
            return Conflict(new { message = e.Message });
        }
    }
}