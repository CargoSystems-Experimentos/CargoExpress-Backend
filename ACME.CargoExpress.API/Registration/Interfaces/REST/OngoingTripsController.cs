using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class OngoingTripsController(IOngoingTripCommandService ongoingTripCommandService, IOngoingTripQueryService ongoingTripQueryService)
    :ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateOngoingTrip([FromBody] CreateOngoingTripResource createOngoingTripResource)
    {
        try
        {
            var createOngoingTripCommand = CreateOngoingTripCommandFromResourceAssembler.ToCommandFromResource(createOngoingTripResource);
            var ongoingTrip = await ongoingTripCommandService.Handle(createOngoingTripCommand);
            if (ongoingTrip is null) return BadRequest();
            var resource = OngoingTripResourceFromEntityAssembler.ToResourceFromEntity(ongoingTrip);
            return CreatedAtAction(nameof(GetOngoingTripById), new { ongoingTripId = resource.Id }, resource);
        }
        catch (InvalidOperationException e)
        {
            // Handle the case where an Expense with the same TripId already exists
            return BadRequest(new {message = "An OnGoingTrip with the same TripId already exists."});
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new { message = "An error occurred while creating the ongoing trip. " + e.Message });
        }
    }
    
    [HttpPut("{ongoingTripId}")]
    public async Task<IActionResult> UpdateOngoingTrip([FromBody] UpdateOngoingTripResource updateOngoingTripResource, [FromRoute] int ongoingTripId)
    {
        try
        {
            var updateOngoingTripCommand = UpdateOngoingTripCommandFromResourceAssembler.ToCommandFromResource(updateOngoingTripResource, ongoingTripId);
            var ongoingTrip = await ongoingTripCommandService.Handle(updateOngoingTripCommand);
            if (ongoingTrip is null) return BadRequest();
            var resource = OngoingTripResourceFromEntityAssembler.ToResourceFromEntity(ongoingTrip);
            return Ok(resource);
        }
        catch (Exception e)
        {
            // Log detailed exception information
            var exceptionDetails = new
            {
                e.Message,
                e.StackTrace,
                InnerExceptionMessage = e.InnerException?.Message,
                InnerExceptionStackTrace = e.InnerException?.StackTrace
            };
            Console.WriteLine(exceptionDetails);
            return BadRequest(new { message = "An error occurred while creating the ongoing trip.", details = exceptionDetails });
        }
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetAllOngoingTrips()
    {
        var getAllOngoingTripsQuery = new GetAllOngoingTripsQuery();
        var ongoingTrips = await ongoingTripQueryService.Handle(getAllOngoingTripsQuery);
        var resources = ongoingTrips.Select(OngoingTripResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("{ongoingTripId}")]
    public async Task<IActionResult> GetOngoingTripById([FromRoute] int ongoingTripId)
    {
        var ongoingTrip = await ongoingTripQueryService.Handle(new GetOnGoingTripByIdQuery(ongoingTripId));
        if (ongoingTrip == null) return NotFound();
        var resource = OngoingTripResourceFromEntityAssembler.ToResourceFromEntity(ongoingTrip);
        return Ok(resource);
    }
    
}