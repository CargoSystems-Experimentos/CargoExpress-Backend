using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class TripsController(
    ITripQueryService tripQueryService,
    ITripCommandService tripCommandService,
    IAuditLogCommandService auditLogCommandService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateTrip([FromBody] CreateTripResource createTripResource)
    {
        try
        {
            var createTripCommand = CreateTripCommandFromResourceAssembler.ToCommandFromResource(createTripResource);
            var trip = await tripCommandService.Handle(createTripCommand);
            if (trip is null) return BadRequest(new { message = "No se pudo crear el viaje." });
            await auditLogCommandService.Handle(new CreateAuditLogCommand("TRIPS", "CREATE", trip.EntrepreneurId,
                new { trip.Id, trip.Name, trip.Type, trip.Weight, trip.LoadLocation, trip.LoadDate, trip.UnloadLocation, trip.UnloadDate, trip.DriverId, trip.VehicleId, trip.ClientId, trip.EntrepreneurId, trip.State }));
            var resource = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
            return CreatedAtAction(nameof(GetTripById), new { tripId = resource.Id }, resource);

        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPut("{tripId}/details")]
    public async Task<IActionResult> UpdateTripDetails([FromBody] UpdateTripDetailsResource resource, [FromRoute] int tripId)
    {
        try
        {
            var command = UpdateTripDetailsCommandFromResourceAssembler.ToCommandFromResource(resource, tripId);
            var trip = await tripCommandService.Handle(command);
            if (trip is null) return NotFound(new { message = "No se ha encontrado el viaje." });
            await auditLogCommandService.Handle(new CreateAuditLogCommand("TRIPS", "UPDATE", trip.EntrepreneurId,
                new { trip.Id, trip.Name, trip.Type, trip.Weight, trip.DriverId, trip.VehicleId, trip.ClientId }));
            var result = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
            return Ok(new
            {
                id = trip.Id,
                name = trip.Name,
                type = trip.Type,
                weight = trip.Weight,
                driverId = trip.DriverId,
                vehicleId = trip.VehicleId,
                clientId = trip.ClientId,
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{tripId}/schedule")]
    public async Task<IActionResult> UpdateTripSchedule([FromBody] UpdateTripScheduleResource resource, [FromRoute] int tripId)
    {
        try
        {
            var command = UpdateTripScheduleCommandFromResourceAssembler.ToCommandFromResource(resource, tripId);
            var trip = await tripCommandService.Handle(command);
            if (trip is null) return NotFound(new { message = "No se ha encontrado el viaje." });
            await auditLogCommandService.Handle(new CreateAuditLogCommand("TRIPS", "UPDATE", trip.EntrepreneurId,
                new { trip.Id, trip.LoadLocation, trip.LoadDate, trip.UnloadLocation, trip.UnloadDate }));
            var result = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
            return Ok(new
            {
                id = trip.Id,
                loadLocation = trip.LoadLocation,
                loadDate = trip.LoadDate,
                unloadLocation = trip.UnloadLocation,
                unloadDate = trip.UnloadDate
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{tripId}/state")]
    public async Task<IActionResult> UpdateTripState([FromBody] UpdateTripStateResource updateTripStateResource, [FromRoute] int tripId)
    {
        try
        {
            var command = new UpdateTripStateCommand(tripId, updateTripStateResource.State);
            var trip = await tripCommandService.Handle(command);
            if (trip is null) return NotFound(new { message = "No se ha encontrado el viaje." });
            await auditLogCommandService.Handle(new CreateAuditLogCommand("TRIPS", "UPDATE", trip.EntrepreneurId,
                new { trip.Id, trip.State }));
            var resource = TripResourceFromEntityAssembler.ToResourceFromEntity(trip);
            return Ok(new
            {
                id = trip.Id,
                state = trip.State
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
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