using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Model.Entities;

namespace ACME.CargoExpress.API.User.Domain.Services;

public interface IEntrepreneurCommandService
{
    Task<Entrepreneur?> Handle(CreateEntrepreneurCommand createEntrepreneurCommand);
    Task<Entrepreneur?> Handle(UpdateEntrepreneurCommand updateEntrepreneurCommand);
}