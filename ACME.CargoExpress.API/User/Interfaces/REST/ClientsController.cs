using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using ACME.CargoExpress.API.User.Domain.Model.Queries;
using ACME.CargoExpress.API.User.Domain.Services;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;
using ACME.CargoExpress.API.User.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.User.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class ClientsController(IClientQueryService clientQueryService, IClientCommandService clientCommandService,
    ITripQueryService tripQueryService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientResource createClientResource)
    {
        try
        {
            var createClientCommand = CreateClientCommandFromResourceAssembler.ToCommandFromResource(createClientResource);
            var client = await clientCommandService.Handle(createClientCommand);
            if (client is null) return BadRequest();
            var resource = ClientResourceFromEntityAssembler.ToResourceFromEntity(client);
            return CreatedAtAction(nameof(GetClientById), new { clientId = resource.Id }, resource);
        }
        catch (Exception e)
        {
            var exceptionDetails = new
            {
                e.Message,
                e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message,
                InnerExceptionStackTrace = e.InnerException?.StackTrace
            };
            Console.WriteLine(exceptionDetails);
            return BadRequest(new { message = "An error occurred while creating the client.", details = exceptionDetails });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllClients()
    {
        var getAllClientsQuery = new GetAllClientsQuery();
        var clients = await clientQueryService.Handle(getAllClientsQuery);
        var resources = clients.Select(ClientResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetClientById([FromRoute] int clientId)
    {
        var client = await clientQueryService.Handle(new GetClientByIdQuery(clientId));
        if (client == null) return NotFound();
        var resource = ClientResourceFromEntityAssembler.ToResourceFromEntity(client);
        return Ok(resource);
    }
    
    [HttpPut("{clientId}")]
    public async Task<IActionResult> UpdateClient([FromBody] UpdateClientResource updateClientResource, [FromRoute] int clientId)
    {
        try
        {
            var updateClientCommand = UpdateClientCommandFromResourceAssembler.ToCommandFromResource(updateClientResource, clientId);
            var client = await clientCommandService.Handle(updateClientCommand);
            if (client is null) return BadRequest();
            var resource = ClientResourceFromEntityAssembler.ToResourceFromEntity(client);
            return Ok(resource);
        }
        catch (Exception e)
        {
            var exceptionDetails = new
            {
                e.Message,
                e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message,
                InnerExceptionStackTrace = e.InnerException?.StackTrace
            };
            Console.WriteLine(exceptionDetails);
            return BadRequest(new { message = "An error occurred while updating the client.", details = exceptionDetails });
        }
    }

    [HttpGet("{clientId}/trips")]
    public async Task<IActionResult> GetTripsByClientId([FromRoute] int clientId)
    {
        var trips = await tripQueryService.Handle(new GetTripsByClientIdQuery(clientId));
        var resources = trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("dni/{dni}")]
    public async Task<IActionResult> GetClientByDni([FromRoute] string dni)
    {
        var client = await clientQueryService.Handle(new GetClientByDniQuery(dni));
        if (client == null) 
            return NotFound(new { message = "Client not found." });

        var resource = ClientResourceFromEntityAssembler.ToResourceFromEntity(client);
        return Ok(resource);
    }
}