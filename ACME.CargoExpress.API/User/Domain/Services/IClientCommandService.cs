using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Model.Entities;

namespace ACME.CargoExpress.API.User.Domain.Services;

public interface IClientCommandService
{
    Task<Client?> Handle(CreateClientCommand createClientCommand);
    Task<Client?> Handle(UpdateClientCommand updateClientCommand);
}