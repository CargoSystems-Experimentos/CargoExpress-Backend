using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using ACME.CargoExpress.API.User.Domain.Exceptions;
using ACME.CargoExpress.API.User.Domain.Model.Queries;
using ACME.CargoExpress.API.User.Domain.Services;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;
using ACME.CargoExpress.API.User.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.User.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class EntrepreneursController(
    IEntrepreneurQueryService entrepreneurQueryService,
    IEntrepreneurCommandService entrepreneurCommandService,
    ITripQueryService tripQueryService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllEntrepreneurs()
    {
        var getAllEntrepreneursQuery = new GetAllEntrepreneursQuery();
        var entrepreneurs = await entrepreneurQueryService.Handle(getAllEntrepreneursQuery);
        var resources = entrepreneurs.Select(EntrepreneurResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{entrepreneurId}")]
    public async Task<IActionResult> GetEntrepreneurById([FromRoute] int entrepreneurId)
    {
        var entrepreneur = await entrepreneurQueryService.Handle(new GetEntrepreneurByIdQuery(entrepreneurId));
        if (entrepreneur == null) return NotFound(new { message = $"No se encontró un emprendedor con el id '{entrepreneurId}'." });
        var resource = EntrepreneurResourceFromEntityAssembler.ToResourceFromEntity(entrepreneur);
        return Ok(resource);
    }

    [HttpPut("{entrepreneurId}")]
    public async Task<IActionResult> UpdateEntrepreneur([FromBody] UpdateEntrepreneurResource updateEntrepreneurResource, [FromRoute] int entrepreneurId)
    {
        try
        {
            var updateEntrepreneurCommand = UpdateEntrepreneurCommandFromResourceAssembler.ToCommandFromResource(updateEntrepreneurResource, entrepreneurId);
            var entrepreneur = await entrepreneurCommandService.Handle(updateEntrepreneurCommand);
            if (entrepreneur is null)
                return BadRequest(new { message = "No se pudo actualizar el emprendedor." });
            var resource = EntrepreneurResourceFromEntityAssembler.ToResourceFromEntity(entrepreneur);
            return Ok(resource);
        }
        catch (InvalidEntrepreneurNameException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidEntrepreneurRucException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (InvalidEntrepreneurAddressException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (DuplicateEntrepreneurNameException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (DuplicateEntrepreneurRucException e)
        {
            return Conflict(new { message = e.Message });
        }
        catch (EntrepreneurNotFoundException e)
        {
            return NotFound(new { message = e.Message });
        }
    }


    [HttpGet("{entrepreneurId}/trips")]
    public async Task<IActionResult> GetTripsByEntrepreneurId([FromRoute] int entrepreneurId)
    {
        var trips = await tripQueryService.Handle(new GetTripsByEntrepreneurIdQuery(entrepreneurId));
        var resources = trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{entrepreneurId}/clients")]
    public async Task<IActionResult> GetClientsByEntrepreneurId([FromRoute] int entrepreneurId)
    {
        var clients = await tripQueryService.Handle(new GetClientsByEntrepreneurId(entrepreneurId));
        var resources = clients.Select(ClientResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
