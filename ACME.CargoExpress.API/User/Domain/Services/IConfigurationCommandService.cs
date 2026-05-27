using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Model.Entities;

namespace ACME.CargoExpress.API.User.Domain.Services;

public interface IConfigurationCommandService
{
    Task<Configuration?> Handle(CreateConfigurationCommand createConfigurationCommand);
    Task<Configuration?> Handle(UpdateConfigurationCommand updateConfigurationCommand);
}