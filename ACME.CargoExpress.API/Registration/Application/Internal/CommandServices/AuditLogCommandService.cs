using System.Text.Json;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class AuditLogCommandService(IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
    : IAuditLogCommandService
{
    private static readonly string[] ValidEntityTypes = ["TRIPS", "ALERTS", "VEHICLES", "DRIVERS", "EXPENSES"];
    private static readonly string[] ValidActions = ["CREATE", "UPDATE", "DELETE"];

    public async Task<AuditLog?> Handle(CreateAuditLogCommand command)
    {
        if (!ValidEntityTypes.Contains(command.EntityType))
            throw new ArgumentException($"Tipo de entidad de auditoría no válido: {command.EntityType}.");

        if (!ValidActions.Contains(command.Action))
            throw new ArgumentException($"Acción de auditoría no válida: {command.Action}.");

        var modifiedFields = command.ModifiedFields is null
            ? "{}"
            : JsonSerializer.Serialize(command.ModifiedFields);

        var auditLog = new AuditLog(command.EntityType, command.Action, modifiedFields, command.EntrepreneurId);

        await auditLogRepository.AddAsync(auditLog);
        await unitOfWork.CompleteAsync();
        return auditLog;
    }
}
