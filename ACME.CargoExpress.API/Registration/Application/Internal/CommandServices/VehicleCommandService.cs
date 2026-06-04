using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Exceptions;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class VehicleCommandService(IVehicleRepository vehicleRepository, IEntrepreneurRepository entrepreneurRepository, IUnitOfWork unitOfWork)
    : IVehicleCommandService
{
    
    private static bool IsValidDecimal10_2(decimal value)
    {
        if (value <= 0)
            return false;

        if (value > 99999999.99m)
            return false;

        var rounded = decimal.Round(value, 2);
        return value == rounded;
    }
    
    public async Task<Vehicle?> Handle(CreateVehicleCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
            throw new InvalidVehicleNameException();

        if (command.Name.Length > 60)
            throw new VehicleNameTooLongException();

        if (string.IsNullOrWhiteSpace(command.Model))
            throw new InvalidVehicleModelException();

        if (string.IsNullOrWhiteSpace(command.Plate))
            throw new InvalidVehiclePlateException();

        if (string.IsNullOrWhiteSpace(command.TractorPlate))
            throw new InvalidVehicleTractorPlateException();

        if (!IsValidDecimal10_2(command.MaxLoad))
            throw new InvalidVehicleMaxLoadRangeException();

        if (!IsValidDecimal10_2(command.Volume))
            throw new InvalidVehicleVolumeRangeException();

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
            throw new ArgumentException("El ID del empresario no fue encontrado.");

        var existingVehicleByName = await vehicleRepository.FindByNameAsync(command.Name);
        if (existingVehicleByName != null)
            throw new DuplicateVehicleNameException(command.Name);

        var existingVehicle = await vehicleRepository.FindByPlateAsync(command.Plate);
        if (existingVehicle != null)
            throw new DuplicateVehiclePlateException(command.Plate);

        var vehicle = new Vehicle(
            command.Name,
            command.Model,
            command.Plate,
            command.TractorPlate,
            command.MaxLoad,
            command.Volume,
            command.EntrepreneurId,
            entrepreneur);

        await vehicleRepository.AddAsync(vehicle);
        await unitOfWork.CompleteAsync();
        return vehicle;
    }

    public async Task<Vehicle?> Handle(UpdateVehicleCommand command)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(command.VehicleId);
        if (vehicle == null)
            return null;

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new InvalidVehicleNameException();

        if (command.Name.Length > 60)
            throw new VehicleNameTooLongException();

        var existingVehicleByName = await vehicleRepository.FindByNameAsync(command.Name);
        if (existingVehicleByName != null && existingVehicleByName.Id != command.VehicleId)
            throw new DuplicateVehicleNameException(command.Name);

        vehicle.Name = command.Name;

        await unitOfWork.CompleteAsync();
        return vehicle;
    }
}


