using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;

namespace ACME.CargoExpress.API.Registration.Domain.Services;

public interface IDriverCommandService
{
    Task<Driver?> Handle(CreateDriverCommand command);
    Task<Driver?> Handle(UpdateDriverCommand command);
}