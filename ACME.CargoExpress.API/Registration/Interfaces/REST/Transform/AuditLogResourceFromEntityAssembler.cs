using System.Text.Json;
using System.Text.Json.Nodes;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.Registration.Interfaces.REST.Transform;

public static class AuditLogResourceFromEntityAssembler
{
    public static AuditLogResource ToResourceFromEntity(AuditLog entity)
    {
        // ModifiedFields is stored as a JSON string; expose it as an embedded JSON object
        // instead of an escaped string in the response.
        JsonNode? modifiedFields = null;
        if (!string.IsNullOrWhiteSpace(entity.ModifiedFields))
        {
            try
            {
                modifiedFields = JsonNode.Parse(entity.ModifiedFields);
            }
            catch (JsonException)
            {
                modifiedFields = JsonValue.Create(entity.ModifiedFields);
            }
        }

        return new AuditLogResource(
            entity.Id,
            entity.EntityType,
            entity.Action,
            entity.Timestamp,
            modifiedFields,
            entity.EntrepreneurId);
    }
}
