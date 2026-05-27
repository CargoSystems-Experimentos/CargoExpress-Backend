using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.User.Application.Internal.CommandServices;

public class ConfigurationCommandService (IConfigurationRepository configurationRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : IConfigurationCommandService
{
    public async Task<Configuration?> Handle(CreateConfigurationCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ArgumentException("UserId not found.");
        }
        // Validate Theme
        if (command.Theme != "Dark" && command.Theme != "Light")
        {
            throw new ArgumentException("Invalid Theme. Only 'Dark' or 'Light' are allowed.");
        }

        // Validate View
        if (command.View != "Grid" && command.View != "List")
        {
            throw new ArgumentException("Invalid View. Only 'Grid' or 'List' are allowed.");
        }
        // Create the configuration
        var configuration = new Configuration(command.UserId, command.Theme, command.View, command.AllowDataCollection, command.UpdateDataSharing, user);
        try
        {
            await configurationRepository.AddAsync(configuration);
            await unitOfWork.CompleteAsync();
            return configuration;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the configuration: {e.Message}");
            return null;
        }
    }
    
    public async Task<Configuration?> Handle(UpdateConfigurationCommand command)
    {
        var configuration = await configurationRepository.FindByIdAsync(command.ConfigurationId);
        
        if (configuration == null)
        {
            return null;
        }
        
        // Validate Theme
        if (command.Theme != "Dark" && command.Theme != "Light")
        {
            throw new ArgumentException("Invalid Theme. Only 'Dark' or 'Light' are allowed.");
        }

        // Validate View
        if (command.View != "Grid" && command.View != "List")
        {
            throw new ArgumentException("Invalid View. Only 'Grid' or 'List' are allowed.");
        }
        
        // Update the configuration information
        
        configuration.Theme = command.Theme;
        configuration.View = command.View;
        configuration.AllowDataCollection = command.AllowDataCollection;
        configuration.UpdateDataSharing = command.UpdateDataSharing;
        
        await unitOfWork.CompleteAsync();
        return configuration;
    }
}