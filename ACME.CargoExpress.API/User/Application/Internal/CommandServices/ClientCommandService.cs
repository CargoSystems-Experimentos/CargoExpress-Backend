using ACME.CargoExpress.API.IAM.Domain.Repositories;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Exceptions;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Commands;
using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Services;

namespace ACME.CargoExpress.API.User.Application.Internal.CommandServices;

public class ClientCommandService(
    IClientRepository clientRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IClientCommandService
{
    public async Task<Client?> Handle(CreateClientCommand command)
    {
        ValidateName(command.Name);
        ValidatePhone(command.Phone);
        ValidateDni(command.Dni);

        if (await clientRepository.FindByPhoneAsync(command.Phone) is not null)
            throw new DuplicateClientPhoneException(command.Phone);

        if (await clientRepository.FindByDniAsync(command.Dni) is not null)
            throw new DuplicateClientDniException(command.Dni);

        var user = await userRepository.FindByIdAsync(command.UserId)
                   ?? throw new UserNotFoundException(command.UserId);

        var client = new Client(command, user);
        await clientRepository.AddAsync(client);
        await unitOfWork.CompleteAsync();
        return client;
    }

    public async Task<Client?> Handle(UpdateClientCommand command)
    {
        ValidateName(command.Name);
        ValidatePhone(command.Phone);
        ValidateDni(command.Dni);

        var client = await clientRepository.FindByIdAsync(command.ClientId)
                     ?? throw new ClientNotFoundException(command.ClientId);

        var existingByPhone = await clientRepository.FindByPhoneAsync(command.Phone);
        if (existingByPhone is not null && existingByPhone.Id != command.ClientId)
            throw new DuplicateClientPhoneException(command.Phone);

        var existingByDni = await clientRepository.FindByDniAsync(command.Dni);
        if (existingByDni is not null && existingByDni.Id != command.ClientId)
            throw new DuplicateClientDniException(command.Dni);

        client.Update(command);
        await unitOfWork.CompleteAsync();
        return client;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidClientNameException("El nombre es obligatorio.");

        if (name.Length < 8 || name.Length > 60)
            throw new InvalidClientNameException("El nombre debe tener entre 8 y 60 caracteres.");
    }

    private static void ValidatePhone(string phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            throw new InvalidClientPhoneException("El teléfono es obligatorio.");

        if (phone.Length != 9)
            throw new InvalidClientPhoneException("El teléfono debe tener exactamente 9 caracteres.");

        if (!phone.All(char.IsDigit))
            throw new InvalidClientPhoneException("El teléfono solo debe contener números.");
    }

    private static void ValidateDni(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni))
            throw new InvalidClientDniException("El DNI es obligatorio.");

        if (dni.Length != 8)
            throw new InvalidClientDniException("El DNI debe tener exactamente 8 caracteres.");

        if (!dni.All(char.IsDigit))
            throw new InvalidClientDniException("El DNI solo debe contener números.");
    }
}
