using System.Net.Mime;
using ACME.CargoExpress.API.IAM.Domain.Model.Queries;
using ACME.CargoExpress.API.IAM.Domain.Services;
using ACME.CargoExpress.API.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using ACME.CargoExpress.API.User.Domain.Model.Queries;
using ACME.CargoExpress.API.User.Domain.Services;
using ACME.CargoExpress.API.User.Interfaces.REST.Transform;
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
public class UsersController(IUserQueryService userQueryService, IUserCommandService userCommandService, 
    IClientQueryService clientQueryService, IEntrepreneurQueryService entrepreneurQueryService,
    IConfigurationQueryService configurationQueryService) : ControllerBase
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
    
    [HttpGet("{userId}/clients")]
    public async Task<IActionResult> GetClientByUserId([FromRoute] int userId)
    {
        var client = await clientQueryService.Handle(new GetClientByUserIdQuery(userId));
        if (client == null) return NotFound();
        var resource = ClientResourceFromEntityAssembler.ToResourceFromEntity(client);
        return Ok(resource);
    }
    
    [HttpGet("{userId}/entrepreneurs")]
    public async Task<IActionResult> GetEntrepreneurByUserId([FromRoute] int userId)
    {
        var entrepreneur = await entrepreneurQueryService.Handle(new GetEntrepreneurByUserIdQuery(userId));
        if (entrepreneur == null) return NotFound();
        var resource = EntrepreneurResourceFromEntityAssembler.ToResourceFromEntity(entrepreneur);
        return Ok(resource);
    }
    
    [HttpGet("{userId}/configurations")]
    public async Task<IActionResult> GetConfigurationByUserId([FromRoute] int userId)
    {
        var configuration = await configurationQueryService.Handle(new GetConfigurationByUserIdQuery(userId));
        if (configuration == null) return NotFound();
        var resource = ConfigurationResourceFromEntityAssembler.ToResourceFromEntity(configuration);
        return Ok(resource);
    }
}