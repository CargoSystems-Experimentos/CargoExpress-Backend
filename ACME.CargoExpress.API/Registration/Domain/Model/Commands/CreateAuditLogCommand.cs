namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

/// <summary>
/// Command used to record an audit log entry. <paramref name="ModifiedFields"/> is an
/// arbitrary object that the command service serializes to JSON.
/// </summary>
public record CreateAuditLogCommand(string EntityType, string Action, int EntrepreneurId, object? ModifiedFields);
