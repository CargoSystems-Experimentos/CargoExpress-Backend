using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class DriversController(IDriverCommandService driverCommandService, IDriverQueryService driverQueryService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDriver([FromBody] CreateDriverResource createDriverResource)
    {
        try
        {
            var createDriverCommand = CreateDriverCommandFromResourceAssembler.ToCommandFromResource(createDriverResource);
            var driver = await driverCommandService.Handle(createDriverCommand);
            if (driver is null) return BadRequest(new { message = "No se pudo crear el conductor." });
            var resource = DriverResourceFromEntityAssembler.ToResourceFromEntity(driver);
            return CreatedAtAction(nameof(GetDriverById), new { driverId = resource.Id }, resource);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{driverId}")]
    public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverResource updateDriverResource, [FromRoute] int driverId)
    {
        try
        {
            var updateDriverCommand = UpdateDriverCommandFromResourceAssembler.ToCommandFromResource(updateDriverResource, driverId);
            var driver = await driverCommandService.Handle(updateDriverCommand);
            if (driver is null) return NotFound(new { message = "No se ha encontrado el conductor." });
            return Ok(new
            {
                id = driver.Id,
                name = driver.Name,
                contactNumber = driver.ContactNumber
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpPut("{driverId}/state")]
    public async Task<IActionResult> UpdateDriverState([FromBody] UpdateDriverStateResource updateDriverStateResource, [FromRoute] int driverId)
    {
        try
        {
            var updateDriverStateCommand = UpdateDriverStateCommandFromResourceAssembler.ToCommandFromResource(updateDriverStateResource, driverId);
            var driver = await driverCommandService.Handle(updateDriverStateCommand);
            if (driver is null) return NotFound(new { message = "No se ha encontrado el conductor." });
            return Ok(new
            {
                id = driver.Id,
                state = driver.State
            });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrivers()
    {
        var getAllDriversQuery = new GetAllDriversQuery();
        var drivers = await driverQueryService.Handle(getAllDriversQuery);
        var resources = drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{driverId}")]
    public async Task<IActionResult> GetDriverById([FromRoute] int driverId)
    {
        var driver = await driverQueryService.Handle(new GetDriverByIdQuery(driverId));
        if (driver == null) return NotFound(new { message = "No se ha encontrado el conductor." });
        var resource = DriverResourceFromEntityAssembler.ToResourceFromEntity(driver);
        return Ok(resource);
    }

}
