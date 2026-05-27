using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IDriverQueryService
{
    Task<Driver?> Handle(GetDriverByIdQuery query);
    Task<IEnumerable<Driver>> Handle(GetAllDriversQuery query);
    Task<IEnumerable<Driver>> Handle(GetDriversByEntrepreneurIdQuery query);
}