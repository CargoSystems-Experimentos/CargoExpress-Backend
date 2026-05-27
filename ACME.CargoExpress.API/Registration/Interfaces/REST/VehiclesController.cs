using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class VehiclesController(IVehicleCommandService vehicleCommandService, IVehicleQueryService vehicleQueryService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleResource createVehicleResource)
    {
        try
        {
            var createVehicleCommand = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(createVehicleResource);
            var vehicle = await vehicleCommandService.Handle(createVehicleCommand);
            if (vehicle is null) return BadRequest();
            var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
            return CreatedAtAction(nameof(GetVehicleById), new { vehicleId = resource.Id }, resource);
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
            return BadRequest(new { message = "An error occurred while creating the vehicle.", details = exceptionDetails });
        }
    }
    
    [HttpPut("{vehicleId}")]
    public async Task<IActionResult> UpdateVehicle([FromBody] UpdateVehicleResource updateVehicleResource, [FromRoute] int vehicleId)
    {
        try
        {
            var updateVehicleCommand = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(updateVehicleResource, vehicleId);
            var vehicle = await vehicleCommandService.Handle(updateVehicleCommand);
            if (vehicle is null) return BadRequest();
            var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
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
            return BadRequest(new { message = "An error occurred while updating the vehicle.", details = exceptionDetails });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllVehicles()
    {
        var getAllVehiclesQuery = new GetAllVehiclesQuery();
        var vehicles = await vehicleQueryService.Handle(getAllVehiclesQuery);
        var resources = vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("{vehicleId}")]
    public async Task<IActionResult> GetVehicleById([FromRoute] int vehicleId)
    {
        var vehicle = await vehicleQueryService.Handle(new GetVehicleByIdQuery(vehicleId));
        if (vehicle == null) return NotFound();
        var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
        return Ok(resource);
    }

    [HttpGet("entrepreneur/{entrepreneurId}")]
    public async Task<IActionResult> GetVehiclesByEntrepreneurId([FromRoute] int entrepreneurId)
    {
        var vehicles = await vehicleQueryService.Handle(new GetVehiclesByEntrepreneurIdQuery(entrepreneurId));
        var resources = vehicles.Select(VehicleResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    
}