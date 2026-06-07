using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class VehiclesController(
    IVehicleCommandService vehicleCommandService,
    IVehicleQueryService vehicleQueryService,
    IAuditLogCommandService auditLogCommandService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleResource createVehicleResource)
    {
        try
        {
            var createVehicleCommand = CreateVehicleCommandFromResourceAssembler.ToCommandFromResource(createVehicleResource);
            var vehicle = await vehicleCommandService.Handle(createVehicleCommand);
            if (vehicle is null) return BadRequest(new { message = "No se pudo crear el vehículo." });
            await auditLogCommandService.Handle(new CreateAuditLogCommand("VEHICLES", "CREATE", vehicle.EntrepreneurId,
                new { vehicle.Id, vehicle.Name, vehicle.Model, vehicle.Plate, vehicle.TractorPlate, vehicle.MaxLoad, vehicle.Volume, vehicle.State, vehicle.EntrepreneurId }));
            var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
            return CreatedAtAction(nameof(GetVehicleById), new { vehicleId = resource.Id }, resource);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPut("{vehicleId}/name")]
    public async Task<IActionResult> UpdateVehicle([FromBody] UpdateVehicleResource updateVehicleResource, [FromRoute] int vehicleId)
    {
        try
        {
            var updateVehicleCommand = UpdateVehicleCommandFromResourceAssembler.ToCommandFromResource(updateVehicleResource, vehicleId);
            var vehicle = await vehicleCommandService.Handle(updateVehicleCommand);
            if (vehicle is null) return BadRequest(new { message = "No se pudo actualizar el nombre del vehículo." });
            await auditLogCommandService.Handle(new CreateAuditLogCommand("VEHICLES", "UPDATE", vehicle.EntrepreneurId,
                new { vehicle.Id, vehicle.Name }));
            return Ok(new
            {
                id = vehicle.Id,
                name = vehicle.Name
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    
    [HttpPut("{vehicleId}/state")]
    public async Task<IActionResult> UpdateVehicleState([FromBody] UpdateVehicleStateResource updateVehicleStateResource, [FromRoute] int vehicleId)
    {
        try
        {
            var updateVehicleStateCommand = UpdateVehicleStateCommandFromResourceAssembler.ToCommandFromResource(updateVehicleStateResource, vehicleId);
            var vehicle = await vehicleCommandService.Handle(updateVehicleStateCommand);
            if (vehicle is null) return NotFound(new { message = "No se ha encontrado el vehículo." });

            var auditAction = vehicle.State == "INACTIVE" ? "DELETE" : "UPDATE";
            await auditLogCommandService.Handle(new CreateAuditLogCommand("VEHICLES", auditAction, vehicle.EntrepreneurId,
                new { vehicle.Id, vehicle.State }));

            return Ok(new
            {
                id = vehicle.Id,
                state = vehicle.State
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
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
        if (vehicle == null) return NotFound(new { message = "No se ha encontrado el vehiculo"});
        var resource = VehicleResourceFromEntityAssembler.ToResourceFromEntity(vehicle);
        return Ok(resource);
    }

}