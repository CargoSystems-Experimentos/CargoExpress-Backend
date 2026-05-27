using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;

namespace ACME.CargoExpress.API.Registration.Application.Internal.QueryServices;

public class AlertQueryService(IAlertRepository alertRepository)
    : IAlertQueryService
{
    public async Task<Alert?> Handle(GetAlertByIdQuery query)
    {
        return await alertRepository.FindByIdAsync(query.AlertId);
    }
    
    public async Task<IEnumerable<Alert>> Handle(GetAllAlertsQuery query)
    {
        return await alertRepository.ListAsync();
    }
}
