using ACME.CargoExpress.API.User.Domain.Model.Queries;
using ACME.CargoExpress.API.User.Domain.Services;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;
using ACME.CargoExpress.API.User.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.User.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ConfigurationsController (IConfigurationQueryService configurationQueryService, IConfigurationCommandService configurationCommandService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateConfiguration([FromBody] CreateConfigurationResource createConfigurationResource)
    {
        try
        {
            var createConfigurationCommand = CreateConfigurationCommandFromResourceAssembler.ToCommandFromResource(createConfigurationResource);
            var configuration = await configurationCommandService.Handle(createConfigurationCommand);
            if (configuration is null) return BadRequest();
            var resource = ConfigurationResourceFromEntityAssembler.ToResourceFromEntity(configuration);
            return CreatedAtAction(nameof(GetConfigurationById), new { configurationId = resource.Id }, resource);
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
            return BadRequest(new { message = "An error occurred while creating the configuration.", details = exceptionDetails });
        }
    }
    
    [HttpPut("{configurationId}")]
    public async Task<IActionResult> UpdateConfiguration([FromBody] UpdateConfigurationResource updateConfigurationResource, [FromRoute] int configurationId)
    {
        try
        {
            var updateConfigurationCommand = UpdateConfigurationCommandFromResourceAssembler.ToCommandFromResource(updateConfigurationResource, configurationId);
            var configuration = await configurationCommandService.Handle(updateConfigurationCommand);
            if (configuration is null) return BadRequest();
            var resource = ConfigurationResourceFromEntityAssembler.ToResourceFromEntity(configuration);
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
            return BadRequest(new { message = "An error occurred while updating the configuration.", details = exceptionDetails });
        }

    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllConfigurations()
    {
        var getAllConfigurationsQuery = new GetAllConfigurationsQuery();
        var configurations = await configurationQueryService.Handle(getAllConfigurationsQuery);
        var resources = configurations.Select(ConfigurationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("{configurationId}")]
    public async Task<IActionResult> GetConfigurationById([FromRoute] int configurationId)
    {
        var configuration = await configurationQueryService.Handle(new GetConfigurationByIdQuery(configurationId));
        if (configuration == null) return NotFound();
        var resource = ConfigurationResourceFromEntityAssembler.ToResourceFromEntity(configuration);
        return Ok(resource);
    }
    
    
    
}