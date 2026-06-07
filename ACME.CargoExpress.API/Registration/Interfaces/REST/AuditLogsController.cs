using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class AuditLogsController(IAuditLogQueryService auditLogQueryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllAuditLogs()
    {
        var auditLogs = await auditLogQueryService.Handle(new GetAllAuditLogsQuery());
        var resources = auditLogs.Select(AuditLogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{auditLogId}")]
    public async Task<IActionResult> GetAuditLogById([FromRoute] Guid auditLogId)
    {
        var auditLog = await auditLogQueryService.Handle(new GetAuditLogByIdQuery(auditLogId));
        if (auditLog is null) return NotFound(new { message = "No se ha encontrado el registro de auditoría." });
        var resource = AuditLogResourceFromEntityAssembler.ToResourceFromEntity(auditLog);
        return Ok(resource);
    }

    [HttpGet("entrepreneur/{entrepreneurId}")]
    public async Task<IActionResult> GetAuditLogsByEntrepreneurId([FromRoute] int entrepreneurId)
    {
        var auditLogs = await auditLogQueryService.Handle(new GetAuditLogsByEntrepreneurIdQuery(entrepreneurId));
        var resources = auditLogs.Select(AuditLogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("entrepreneur/alerts/{entrepreneurId}")]
    public Task<IActionResult> GetAlertAuditLogsByEntrepreneurId([FromRoute] int entrepreneurId) =>
        GetAuditLogsByEntityType(entrepreneurId, "ALERTS");

    [HttpGet("entrepreneur/expenses/{entrepreneurId}")]
    public Task<IActionResult> GetExpenseAuditLogsByEntrepreneurId([FromRoute] int entrepreneurId) =>
        GetAuditLogsByEntityType(entrepreneurId, "EXPENSES");

    [HttpGet("entrepreneur/drivers/{entrepreneurId}")]
    public Task<IActionResult> GetDriverAuditLogsByEntrepreneurId([FromRoute] int entrepreneurId) =>
        GetAuditLogsByEntityType(entrepreneurId, "DRIVERS");

    [HttpGet("entrepreneur/trips/{entrepreneurId}")]
    public Task<IActionResult> GetTripAuditLogsByEntrepreneurId([FromRoute] int entrepreneurId) =>
        GetAuditLogsByEntityType(entrepreneurId, "TRIPS");

    [HttpGet("entrepreneur/vehicles/{entrepreneurId}")]
    public Task<IActionResult> GetVehicleAuditLogsByEntrepreneurId([FromRoute] int entrepreneurId) =>
        GetAuditLogsByEntityType(entrepreneurId, "VEHICLES");

    private async Task<IActionResult> GetAuditLogsByEntityType(int entrepreneurId, string entityType)
    {
        var auditLogs = await auditLogQueryService.Handle(
            new GetAuditLogsByEntrepreneurIdAndEntityTypeQuery(entrepreneurId, entityType));
        var resources = auditLogs.Select(AuditLogResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
