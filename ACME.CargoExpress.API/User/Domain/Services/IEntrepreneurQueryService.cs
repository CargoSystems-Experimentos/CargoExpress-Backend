using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Model.Queries;

namespace ACME.CargoExpress.API.User.Domain.Services;

public interface IEntrepreneurQueryService
{
    Task<IEnumerable<Entrepreneur>> Handle(GetAllEntrepreneursQuery query);
    Task<Entrepreneur?> Handle(GetEntrepreneurByIdQuery query);
    Task<Entrepreneur?> Handle(GetEntrepreneurByUserIdQuery query);
}