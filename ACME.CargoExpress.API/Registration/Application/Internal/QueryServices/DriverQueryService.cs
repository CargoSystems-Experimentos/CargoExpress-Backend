using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;

namespace ACME.CargoExpress.API.Registration.Application.Internal.QueryServices;

public class DriverQueryService(IDriverRepository driverRepository)
    : IDriverQueryService
{
    public async Task<Driver?> Handle(GetDriverByIdQuery query)
    {
        return await driverRepository.FindByIdAsync(query.DriverId);
    }
    
    public async Task<IEnumerable<Driver>> Handle(GetAllDriversQuery query)
    {
        return await driverRepository.ListAsync();
    }

    public async Task<IEnumerable<Driver>> Handle(GetDriversByEntrepreneurIdQuery query)
    {
        return await driverRepository.FindByEntrepreneurIdAsync(query.EntrepreneurId);
    }
}

