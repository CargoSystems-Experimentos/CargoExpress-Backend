namespace ACME.CargoExpress.API.Registration.Domain.Model.Queries;

public record GetAuditLogsByEntrepreneurIdAndEntityTypeQuery(int EntrepreneurId, string EntityType);
