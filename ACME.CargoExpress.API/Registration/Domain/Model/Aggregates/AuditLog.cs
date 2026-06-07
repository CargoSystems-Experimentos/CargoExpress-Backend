using ACME.CargoExpress.API.User.Domain.Model.Aggregates;

namespace ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;

/// <summary>
/// Audit trail record automatically generated when a tracked entity (Trip, Alert,
/// Vehicle, Driver, Expense) is created, updated or logically deleted.
/// </summary>
public class AuditLog
{
    public AuditLog()
    {
        Id = Guid.NewGuid();
        EntityType = string.Empty;
        Action = string.Empty;
        Timestamp = DateTime.UtcNow;
        ModifiedFields = "{}";
    }

    public AuditLog(string entityType, string action, string modifiedFields, int entrepreneurId)
    {
        Id = Guid.NewGuid();
        EntityType = entityType;
        Action = action;
        Timestamp = DateTime.UtcNow;
        ModifiedFields = modifiedFields;
        EntrepreneurId = entrepreneurId;
    }

    public Guid Id { get; set; }

    /// <summary>TRIPS, ALERTS, VEHICLES, DRIVERS or EXPENSES.</summary>
    public string EntityType { get; set; }

    /// <summary>CREATE, UPDATE or DELETE.</summary>
    public string Action { get; set; }

    public DateTime Timestamp { get; set; }

    /// <summary>JSON document describing the fields involved in the change.</summary>
    public string ModifiedFields { get; set; }

    public int EntrepreneurId { get; set; }
    public Entrepreneur? Entrepreneur { get; set; }
}
