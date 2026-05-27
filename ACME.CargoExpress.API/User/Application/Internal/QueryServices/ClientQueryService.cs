using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Model.Queries;
using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.User.Application.Internal.QueryServices;

public class ClientQueryService(IClientRepository clientRepository) : IClientQueryService
{
    public async Task<IEnumerable<Client>> Handle(GetAllClientsQuery query)
    {
        return await clientRepository.ListAsync();
    }
    
    public async Task<Client?> Handle(GetClientByIdQuery query)
    {
        return await clientRepository.FindByIdAsync(query.ClientId);
    }

    public async Task<Client?> Handle(GetClientByUserIdQuery query)
    {
        return await clientRepository.FindByUserIdAsync(query.UserId);
    }
    public async Task<Client?> Handle(GetClientByDniQuery query)
        => await clientRepository.FindByDniAsync(query.Dni);
}