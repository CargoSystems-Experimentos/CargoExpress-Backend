using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Model.Queries;
using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.User.Application.Internal.QueryServices;

public class EntrepreneurQueryService(IEntrepreneurRepository entrepreneurRepository) : IEntrepreneurQueryService
{
    public async Task<IEnumerable<Entrepreneur>> Handle(GetAllEntrepreneursQuery query)
    {
        return await entrepreneurRepository.ListAsync();
    }
    
    public async Task<Entrepreneur?> Handle(GetEntrepreneurByIdQuery query)
    {
        return await entrepreneurRepository.FindByIdAsync(query.EntrepreneurId);
    }
    
    public async Task<Entrepreneur?> Handle(GetEntrepreneurByUserIdQuery query)
    {
        return await entrepreneurRepository.FindByUserIdAsync(query.UserId);
    }
}
