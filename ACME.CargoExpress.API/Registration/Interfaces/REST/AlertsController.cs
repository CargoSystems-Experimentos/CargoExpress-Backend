using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class AlertsController(
    IAlertCommandService alertCommandService,
    IAlertQueryService alertQueryService,
    ITripQueryService tripQueryService,
    IAuditLogCommandService auditLogCommandService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAlert([FromBody] CreateAlertResource createAlertResource)
    {
        try
        {
            var createAlertCommand = CreateAlertCommandFromResourceAssembler.ToCommandFromResource(createAlertResource);
            var alert = await alertCommandService.Handle(createAlertCommand);
            if (alert is null) return BadRequest(new { message = "No se pudo crear la alerta." });
            var trip = await tripQueryService.Handle(new GetTripByIdQuery(alert.TripId));
            if (trip is not null)
                await auditLogCommandService.Handle(new CreateAuditLogCommand("ALERTS", "CREATE", trip.EntrepreneurId,
                    new { alert.Id, alert.Title, alert.Type, alert.Description, alert.Date, alert.TripId }));
            var resource = AlertResourceFromEntityAssembler.ToResourceFromEntity(alert);
            return CreatedAtAction(nameof(GetAlertById), new { alertId = resource.Id }, resource);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAlerts()
    {
        var getAllAlertsQuery = new GetAllAlertsQuery();
        var alerts = await alertQueryService.Handle(getAllAlertsQuery);
        var resources = alerts.Select(AlertResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{alertId}")]
    public async Task<IActionResult> GetAlertById([FromRoute] int alertId)
    {
        var alert = await alertQueryService.Handle(new GetAlertByIdQuery(alertId));
        if (alert is null) return NotFound(new { message = "No se ha encontrado la alerta." });
        var resource = AlertResourceFromEntityAssembler.ToResourceFromEntity(alert);
        return Ok(resource);
    }
}
