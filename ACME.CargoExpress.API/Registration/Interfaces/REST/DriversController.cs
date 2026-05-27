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
            if (driver is null) return BadRequest();
            var resource = DriverResourceFromEntityAssembler.ToResourceFromEntity(driver);
            return CreatedAtAction(nameof(GetDriverById), new { driverId = resource.Id }, resource);
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
            return BadRequest(new { message = "An error occurred while updating the driver.", details = exceptionDetails });
        }
    }
    
    [HttpPut("{driverId}")]
    public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverResource updateDriverResource, [FromRoute] int driverId)
    {
        try
        {
            var updateDriverCommand = UpdateDriverCommandFromResourceAssembler.ToCommandFromResource(updateDriverResource, driverId);
            var driver = await driverCommandService.Handle(updateDriverCommand);
            if (driver is null) return BadRequest();
            var resource = DriverResourceFromEntityAssembler.ToResourceFromEntity(driver);
            return Ok(resource);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(new { message = "An error occurred while updating the driver. " + e.Message });
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
        if (driver == null) return NotFound();
        var resource = DriverResourceFromEntityAssembler.ToResourceFromEntity(driver);
        return Ok(resource);
    }

    [HttpGet("entrepreneur/{entrepreneurId}")]
    public async Task<IActionResult> GetDriversByEntrepreneurId([FromRoute] int entrepreneurId)
    {
        var drivers = await driverQueryService.Handle(new GetDriversByEntrepreneurIdQuery(entrepreneurId));
        var resources = drivers.Select(DriverResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
}