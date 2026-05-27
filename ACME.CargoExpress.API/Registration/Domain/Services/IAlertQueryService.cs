using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IAlertQueryService
{
    Task<Alert?> Handle(GetAlertByIdQuery query);
    Task<IEnumerable<Alert>> Handle(GetAllAlertsQuery query);
}