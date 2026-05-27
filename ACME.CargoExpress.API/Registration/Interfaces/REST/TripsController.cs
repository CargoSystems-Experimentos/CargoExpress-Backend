using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class TripsController(ITripQueryService tripQueryService, ITripCommandService tripCommandService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripResource createTripResource)
    {
        try
        {
            var createTripCommand = CreateTripCommandFromResourceAssembler.ToCommandFromResource(createTripResource);
            var trip = await tripCommandService.Handle(createTripCommand);
            if (trip is null) return BadRequest();
            var resource = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
            return CreatedAtAction(nameof(GetTripById), new { tripId = resource.Id }, resource);
            
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
            return BadRequest(new { message = "An error occurred while creating the trip.", details = exceptionDetails });
        }
    }
    
    [HttpPut("{tripId}")]
    public async Task<IActionResult> UpdateTrip([FromBody] UpdateTripResource updateTripResource, [FromRoute] int tripId)
    {
        try
        {
            var updateTripCommand = UpdateTripCommandFromResourceAssembler.ToCommandFromResource(updateTripResource, tripId);
            var trip = await tripCommandService.Handle(updateTripCommand);
            if (trip is null) return BadRequest();
            var resource = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
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
            return BadRequest(new { message = "An error occurred while updating the trip.", details = exceptionDetails });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTrips()
    {
        var getAllTripsQuery = new GetAllTripsQuery();
        var trips = await tripQueryService.Handle(getAllTripsQuery);
        var resources = trips.Select(TripResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    
    
    [HttpGet("{tripId}")]
    public async Task<IActionResult> GetTripById([FromRoute] int tripId)
    {
        var trip = await tripQueryService.Handle(new GetTripByIdQuery(tripId));
        if (trip == null) return NotFound();
        var resource = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
        return Ok(resource);
    }
    
    [HttpGet("{tripId}/alerts")]
    public async Task<IActionResult> GetAlertsByTripId([FromRoute] int tripId)
    {
        var alerts = await tripQueryService.Handle(new GetAlertsByOngoingTripIdQuery(tripId));
        var resources = alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("{tripId}/ongoing-trips")]
    public async Task<IActionResult> GetOngoingByTripId([FromRoute] int tripId)
    {
        var ongoingTrip = await tripQueryService.Handle(new GetOngGoingTripByIdQuery(tripId));
        if (ongoingTrip == null) return NotFound();
        var resource = OngoingTripResourceFromEntityAssembler.ToResourceFromEntity(ongoingTrip);
        return Ok(resource);
    }
    
    [HttpGet("{tripId}/expenses")]
    public async Task<IActionResult> GetExpensesByTripId([FromRoute] int tripId)
    {
        var expenses = await tripQueryService.Handle(new GetExpensesByTripIdQuery(tripId));
        if (expenses == null) return NotFound();
        var resources = ExpenseResourceFromEntityAssembler.ToResourceFromEntity(expenses);
        return Ok(resources);
    }
    
}