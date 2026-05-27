using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Model.Queries;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IVehicleQueryService
{
    Task<Vehicle?> Handle(GetVehicleByIdQuery query);
    Task<IEnumerable<Vehicle>> Handle(GetAllVehiclesQuery query);
    Task<IEnumerable<Vehicle>> Handle(GetVehiclesByEntrepreneurIdQuery query);
}