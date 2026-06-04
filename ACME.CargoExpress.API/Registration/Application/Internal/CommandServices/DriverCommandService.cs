using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Exceptions;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class DriverCommandService(IDriverRepository driverRepository, IEntrepreneurRepository entrepreneurRepository, IUnitOfWork unitOfWork)
    : IDriverCommandService
{
    private static void ValidateDriver(string name, string dni, string license, string contactNumber)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new InvalidDriverNameException();

        if (name.Length > 100)
            throw new DriverNameTooLongException();

        if (!dni.All(char.IsDigit))
            throw new InvalidDriverDniFormatException();

        if (dni.Length != 8)
            throw new InvalidDriverDniLengthException();

        if (string.IsNullOrWhiteSpace(license))
            throw new InvalidDriverLicenseException();

        if (!contactNumber.All(char.IsDigit))
            throw new InvalidDriverPhoneFormatException();

        if (contactNumber.Length != 9)
            throw new InvalidDriverPhoneLengthException();
    }

    public async Task<Driver?> Handle(CreateDriverCommand command)
    {
        ValidateDriver(command.Name, command.Dni, command.License, command.ContactNumber);

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
            throw new ArgumentException("El ID del empresario no fue encontrado.");

        var existingDriverByName = await driverRepository.FindByNameAsync(command.Name);
        if (existingDriverByName != null)
            throw new DuplicateDriverNameException();

        var existingDriverByDni = await driverRepository.FindByDniAsync(command.Dni);
        if (existingDriverByDni != null)
            throw new ArgumentException("El DNI del conductor ya está registrado.");

        var driver = new Driver(
            command.Name,
            command.Dni,
            command.License,
            command.ContactNumber,
            command.EntrepreneurId,
            entrepreneur);

        await driverRepository.AddAsync(driver);
        await unitOfWork.CompleteAsync();
        return driver;
    }

    public async Task<Driver?> Handle(UpdateDriverCommand command)
    {
        var driver = await driverRepository.FindByIdAsync(command.DriverId);
        if (driver == null)
            return null;

        ValidateDriver(command.Name, command.Dni, command.License, command.ContactNumber);

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
            throw new ArgumentException("El ID del empresario no fue encontrado.");

        var existingDriverByName = await driverRepository.FindByNameAsync(command.Name);
        if (existingDriverByName != null && existingDriverByName.Id != command.DriverId)
            throw new DuplicateDriverNameException();

        var existingDriverByDni = await driverRepository.FindByDniAsync(command.Dni);
        if (existingDriverByDni != null && existingDriverByDni.Id != command.DriverId)
            throw new ArgumentException("El DNI del conductor ya está registrado.");

        driver.Name = command.Name;
        driver.Dni = command.Dni;
        driver.License = command.License;
        driver.ContactNumber = command.ContactNumber;
        driver.EntrepreneurId = command.EntrepreneurId;
        driver.Entrepreneur = entrepreneur;

        await unitOfWork.CompleteAsync();
        return driver;
    }
}
