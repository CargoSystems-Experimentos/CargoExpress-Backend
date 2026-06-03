using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class VehicleCommandService(IVehicleRepository vehicleRepository, IEntrepreneurRepository entrepreneurRepository, IUnitOfWork unitOfWork)
    : IVehicleCommandService
{
    public async Task<Vehicle?> Handle(CreateVehicleCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("El nombre del vehículo no puede estar vacío.");
        }

        if (command.Name.Length > 60)
        {
            throw new ArgumentException("El nombre del vehículo no puede exceder 60 caracteres.");
        }

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
        {
            throw new ArgumentException("El ID del empresario no fue encontrado.");
        }

        var existingVehicleByName = await vehicleRepository.FindByNameAsync(command.Name);
        if (existingVehicleByName != null)
        {
            throw new ArgumentException("El nombre del vehículo ya está registrado.");
        }

        var existingVehicle = await vehicleRepository.FindByPlateAsync(command.Plate);
        if (existingVehicle != null)
        {
            throw new ArgumentException("La placa del vehículo ya está registrada.");
        }

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
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new ArgumentException("El nombre del vehículo no puede estar vacío.");
        }

        if (command.Name.Length > 60)
        {
            throw new ArgumentException("El nombre del vehículo no puede exceder 60 caracteres.");
        }

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
        {
            throw new ArgumentException("El ID del empresario no fue encontrado.");
        }

        var existingVehicleByName = await vehicleRepository.FindByNameAsync(command.Name);
        if (existingVehicleByName != null && existingVehicleByName.Id != command.VehicleId)
        {
            throw new ArgumentException("El nombre del vehículo ya está registrado.");
        }

        var existingVehicle = await vehicleRepository.FindByPlateAsync(command.Plate);
        if (existingVehicle != null && existingVehicle.Id != command.VehicleId)
        {
            throw new ArgumentException("La placa del vehículo ya está registrada.");
        }

        vehicle.Name = command.Name;
        vehicle.Model = command.Model;
        vehicle.Plate = command.Plate;
        vehicle.TractorPlate = command.TractorPlate;
        vehicle.MaxLoad = command.MaxLoad;
        vehicle.Volume = command.Volume;
        vehicle.EntrepreneurId = command.EntrepreneurId;
        vehicle.Entrepreneur = entrepreneur;

        await unitOfWork.CompleteAsync();
        return vehicle;
    }
}