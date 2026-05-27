using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.User.Application.Internal.CommandServices;

public class ClientCommandService(IClientRepository clientRepository, IUserRepository userRepository, IUnitOfWork unitOfWork) : IClientCommandService
{
    public async Task<Client?> Handle(CreateClientCommand command)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(command.Phone) || command.Phone.Length != 9)
        {
            throw new ArgumentException("Phone must have exactly 9 characters.");
        }
        
        if (string.IsNullOrWhiteSpace(command.Dni) || command.Dni.Length != 8)
        {
            throw new ArgumentException("Dni must have exactly 8 characters.");
        }
        
        var existingClientByDni = await clientRepository.FindByDniAsync(command.Dni);
        if (existingClientByDni != null)
        {
            throw new ArgumentException("Client DNI is already registered.");
        }

        var existingClientByPhone = await clientRepository.FindByPhoneAsync(command.Phone);
        if (existingClientByPhone != null)
        {
            throw new ArgumentException("Client phone number is already registered.");
        }

        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ArgumentException("UserId not found.");
        }
        // Create the client
        var client = new Client(command, user);
        try
        {
            await clientRepository.AddAsync(client);
            await unitOfWork.CompleteAsync();
            return client;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the client: {e.Message}");
            return null;
        }
    }
    
    public async Task<Client?> Handle(UpdateClientCommand command)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(command.Phone) || command.Phone.Length != 9)
        {
            throw new ArgumentException("Phone must have exactly 9 characters.");
        }
        
        if (string.IsNullOrWhiteSpace(command.Dni) || command.Dni.Length != 8)
        {
            throw new ArgumentException("Dni must have exactly 8 characters.");
        }
        
        var client = await clientRepository.FindByIdAsync(command.ClientId);
        if (client == null)
        {
            throw new ArgumentException("Client not found.");
        }

        var existingClientByDni = await clientRepository.FindByDniAsync(command.Dni);
        if (existingClientByDni != null && existingClientByDni.Id != command.ClientId)
        {
            throw new ArgumentException("Client DNI is already registered.");
        }

        var existingClientByPhone = await clientRepository.FindByPhoneAsync(command.Phone);
        if (existingClientByPhone != null && existingClientByPhone.Id != command.ClientId)
        {
            throw new ArgumentException("Client phone number is already registered.");
        }

        // Update the client
        client.Update(command);
        try
        {
            await unitOfWork.CompleteAsync();
            return client;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while updating the client: {e.Message}");
            return null;
        }
    }
}