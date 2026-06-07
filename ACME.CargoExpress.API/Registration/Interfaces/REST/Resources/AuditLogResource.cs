using System.Text.Json.Nodes;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

public record AuditLogResource(
    Guid Id,
    string EntityType,
    string Action,
    DateTime Timestamp,
    JsonNode? ModifiedFields,
    int EntrepreneurId);
