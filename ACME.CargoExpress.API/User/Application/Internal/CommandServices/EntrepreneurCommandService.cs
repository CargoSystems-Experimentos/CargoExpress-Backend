using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Exceptions;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.User.Application.Internal.CommandServices;

public class EntrepreneurCommandService(
    IEntrepreneurRepository entrepreneurRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IEntrepreneurCommandService
{
    public async Task<Entrepreneur?> Handle(CreateEntrepreneurCommand command)
    {
        ValidateName(command.Name);
        ValidateRuc(command.Ruc);
        ValidateAddress(command.Address);

        if (await entrepreneurRepository.FindByNameAsync(command.Name) is not null)
            throw new DuplicateEntrepreneurNameException(command.Name);

        if (await entrepreneurRepository.FindByRucAsync(command.Ruc) is not null)
            throw new DuplicateEntrepreneurRucException(command.Ruc);

        var user = await userRepository.FindByIdAsync(command.UserId)
                   ?? throw new UserNotFoundException(command.UserId);

        var entrepreneur = new Entrepreneur(command, user);
        await entrepreneurRepository.AddAsync(entrepreneur);
        await unitOfWork.CompleteAsync();
        return entrepreneur;
    }

    public async Task<Entrepreneur?> Handle(UpdateEntrepreneurCommand command)
    {
        ValidateName(command.Name);
        ValidateRuc(command.Ruc);
        ValidateAddress(command.Address);

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId)
                           ?? throw new EntrepreneurNotFoundException(command.EntrepreneurId);

        var existingByName = await entrepreneurRepository.FindByNameAsync(command.Name);
        if (existingByName is not null && existingByName.Id != command.EntrepreneurId)
            throw new DuplicateEntrepreneurNameException(command.Name);

        var existingByRuc = await entrepreneurRepository.FindByRucAsync(command.Ruc);
        if (existingByRuc is not null && existingByRuc.Id != command.EntrepreneurId)
            throw new DuplicateEntrepreneurRucException(command.Ruc);

        entrepreneur.Update(command);
        await unitOfWork.CompleteAsync();
        return entrepreneur;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidEntrepreneurNameException("El nombre es obligatorio.");

        if (name.Length < 8 || name.Length > 60)
            throw new InvalidEntrepreneurNameException("El nombre debe tener entre 8 y 60 caracteres.");
    }

    private static void ValidateRuc(string ruc)
    {
        if (string.IsNullOrWhiteSpace(ruc))
            throw new InvalidEntrepreneurRucException("El RUC es obligatorio.");

        if (ruc.Length != 11)
            throw new InvalidEntrepreneurRucException("El RUC debe tener exactamente 11 caracteres.");

        if (!ruc.All(char.IsDigit))
            throw new InvalidEntrepreneurRucException("El RUC solo debe contener números.");
    }

    private static void ValidateAddress(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new InvalidEntrepreneurAddressException("La dirección es obligatoria.");

        if (address.Length > 200)
            throw new InvalidEntrepreneurAddressException("La dirección no debe exceder 200 caracteres.");
    }
}
