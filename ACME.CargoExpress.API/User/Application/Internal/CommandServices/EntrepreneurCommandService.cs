using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.User.Application.Internal.CommandServices;

public class EntrepreneurCommandService(IEntrepreneurRepository entrepreneurRepository, IUserRepository userRepository, IUnitOfWork unitOfWork): IEntrepreneurCommandService
{
    public async Task<Entrepreneur?> Handle(CreateEntrepreneurCommand command)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(command.Phone) || command.Phone.Length != 9)
        {
            throw new ArgumentException("Phone must have exactly 9 characters.");
        }
        
        if (string.IsNullOrWhiteSpace(command.Ruc) || command.Ruc.Length != 11)
        {
            throw new ArgumentException("Ruc must have exactly 11 characters.");
        }
        
        var existingEntrepreneurByPhone = await entrepreneurRepository.FindByPhoneAsync(command.Phone);
        if (existingEntrepreneurByPhone != null)
        {
            throw new ArgumentException("Entrepreneur phone number is already registered.");
        }

        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null)
        {
            throw new ArgumentException("UserId not found.");
        }
        // Create the entrepreneur
        var entrepreneur = new Entrepreneur(command, user);
        try
        {
            await entrepreneurRepository.AddAsync(entrepreneur);
            await unitOfWork.CompleteAsync();
            return entrepreneur;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while creating the entrepreneur: {e.Message}");
            return null;
        }
    }
    
    public async Task<Entrepreneur?> Handle(UpdateEntrepreneurCommand command)
    {
        // Validaciones
        if (string.IsNullOrWhiteSpace(command.Phone) || command.Phone.Length != 9)
        {
            throw new ArgumentException("Phone must have exactly 9 characters.");
        }
        
        if (string.IsNullOrWhiteSpace(command.Ruc) || command.Ruc.Length != 11)
        {
            throw new ArgumentException("Ruc must have exactly 11 characters.");
        }
        
        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
        {
            throw new ArgumentException("Entrepreneur not found.");
        }

        var existingEntrepreneurByPhone = await entrepreneurRepository.FindByPhoneAsync(command.Phone);
        if (existingEntrepreneurByPhone != null && existingEntrepreneurByPhone.Id != command.EntrepreneurId)
        {
            throw new ArgumentException("Entrepreneur phone number is already registered.");
        }

        // Update the entrepreneur
        entrepreneur.Update(command);
        try
        {
            await unitOfWork.CompleteAsync();
            return entrepreneur;
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred while updating the entrepreneur: {e.Message}");
            return null;
        }
    }
}