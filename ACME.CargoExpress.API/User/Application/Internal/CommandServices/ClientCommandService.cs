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
        ValidateDni(command.Dni);
        ValidateBirthDate(command.BirthDate);

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
        ValidateDni(command.Dni);
        ValidateBirthDate(command.BirthDate);

        var client = await clientRepository.FindByIdAsync(command.ClientId)
                     ?? throw new ClientNotFoundException(command.ClientId);

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

    private static void ValidateDni(string dni)
    {
        if (string.IsNullOrWhiteSpace(dni))
            throw new InvalidClientDniException("El DNI es obligatorio.");

        if (dni.Length != 8)
            throw new InvalidClientDniException("El DNI debe tener exactamente 8 caracteres.");

        if (!dni.All(char.IsDigit))
            throw new InvalidClientDniException("El DNI solo debe contener números.");
    }

    private static void ValidateBirthDate(DateTime birthDate)
    {
        if (birthDate == DateTime.MinValue)
            throw new InvalidClientBirthDateException("La fecha de nacimiento es obligatoria.");

        var today = DateTime.Today;
        if (birthDate > today)
            throw new InvalidClientBirthDateException("La fecha de nacimiento no puede ser en el futuro.");

        var age = today.Year - birthDate.Year;
        if (birthDate.Date > today.AddYears(-age))
            age--;

        if (age < 18)
            throw new InvalidClientBirthDateException("El cliente debe tener al menos 18 años de edad.");
    }
}
