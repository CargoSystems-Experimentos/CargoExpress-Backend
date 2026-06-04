using ACME.CargoExpress.API.User.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class TripCommandService(
    ITripRepository tripRepository,
    IDriverRepository driverRepository,
    IVehicleRepository vehicleRepository,
    IClientRepository clientRepository,
    IEntrepreneurRepository entrepreneurRepository,
    IUnitOfWork unitOfWork)
    : ITripCommandService
{
    public async Task<Trip?> Handle(CreateTripCommand command)
    {
        ValidateFields(command.Name, command.Type, command.Weight, command.LoadLocation,
            command.LoadDate, command.UnloadLocation, command.UnloadDate);

        var client = await clientRepository.FindByIdAsync(command.ClientId);
        if (client == null)
            throw new ArgumentException("El ID del cliente no fue encontrado.");

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
            throw new ArgumentException("El ID del empresario no fue encontrado.");

        var existingTrip = await tripRepository.FindByNameAsync(command.Name, entrepreneur.Id);
        if (existingTrip != null)
            throw new ArgumentException("El nombre del viaje ya está registrado para este empresario.");

        var driver = await ValidateDriverAsync(command.DriverId, entrepreneur.Id);
        var vehicle = await ValidateVehicleAsync(command.VehicleId, entrepreneur.Id);

        var trip = new Trip(command, driver, vehicle, client, entrepreneur);

        await tripRepository.AddAsync(trip);
        await unitOfWork.CompleteAsync();
        return trip;
    }

    public async Task<Trip?> Handle(UpdateTripCommand command)
    {
        var trip = await tripRepository.FindByIdAsync(command.TripId);
        if (trip == null)
            return null;

        ValidateFields(command.Name, command.Type, command.Weight, command.LoadLocation,
            command.LoadDate, command.UnloadLocation, command.UnloadDate);

        var client = await clientRepository.FindByIdAsync(command.ClientId);
        if (client == null)
            throw new ArgumentException("El ID del cliente no fue encontrado.");

        var entrepreneur = await entrepreneurRepository.FindByIdAsync(command.EntrepreneurId);
        if (entrepreneur == null)
            throw new ArgumentException("El ID del empresario no fue encontrado.");

        var existingTrip = await tripRepository.FindByNameAsync(command.Name, entrepreneur.Id);
        if (existingTrip != null && existingTrip.Id != command.TripId)
            throw new ArgumentException("El nombre del viaje ya está registrado para este empresario.");

        var driver = await ValidateDriverAsync(command.DriverId, entrepreneur.Id);
        var vehicle = await ValidateVehicleAsync(command.VehicleId, entrepreneur.Id);

        trip.Name = command.Name;
        trip.Type = command.Type;
        trip.Weight = command.Weight;
        trip.LoadLocation = command.LoadLocation;
        trip.LoadDate = command.LoadDate;
        trip.UnloadLocation = command.UnloadLocation;
        trip.UnloadDate = command.UnloadDate;
        trip.DriverId = driver.Id;
        trip.VehicleId = vehicle.Id;
        trip.ClientId = command.ClientId;
        trip.EntrepreneurId = command.EntrepreneurId;
        trip.Driver = driver;
        trip.Vehicle = vehicle;
        trip.Client = client;
        trip.Entrepreneur = entrepreneur;

        await unitOfWork.CompleteAsync();
        return trip;
    }

    private static void ValidateFields(string name, string type, decimal weight, string loadLocation,
        DateTime loadDate, string unloadLocation, DateTime unloadDate)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre del viaje no puede estar vacío.");
        if (name.Length > 60)
            throw new ArgumentException("El nombre del viaje no puede exceder 60 caracteres.");

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("El tipo del viaje no puede estar vacío.");
        if (type.Length > 60)
            throw new ArgumentException("El tipo del viaje no puede exceder 60 caracteres.");

        if (!IsValidDecimal10_2(weight))
            throw new ArgumentException("El peso del viaje debe ser mayor a 0 y tener como máximo 2 decimales.");

        if (string.IsNullOrWhiteSpace(loadLocation))
            throw new ArgumentException("El lugar de carga no puede estar vacío.");
        if (loadLocation.Length > 100)
            throw new ArgumentException("El lugar de carga no puede exceder 100 caracteres.");

        if (string.IsNullOrWhiteSpace(unloadLocation))
            throw new ArgumentException("El lugar de descarga no puede estar vacío.");
        if (unloadLocation.Length > 100)
            throw new ArgumentException("El lugar de descarga no puede exceder 100 caracteres.");

        if (string.Equals(loadLocation.Trim(), unloadLocation.Trim(), StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("El lugar de carga no puede ser igual al lugar de descarga.");

        if (loadDate == default)
            throw new ArgumentException("La fecha de carga no puede estar vacía.");
        if (unloadDate == default)
            throw new ArgumentException("La fecha de descarga no puede estar vacía.");

        if (loadDate >= unloadDate)
            throw new ArgumentException("La fecha y hora de carga debe ser anterior a la fecha y hora de descarga.");
    }

    private async Task<Driver> ValidateDriverAsync(int driverId, int entrepreneurId)
    {
        var driver = await driverRepository.FindByIdAsync(driverId);
        if (driver == null)
            throw new ArgumentException("El ID del conductor no fue encontrado.");
        if (driver.EntrepreneurId != entrepreneurId)
            throw new ArgumentException("El conductor no pertenece al empresario indicado.");
        if (driver.State != "AVAILABLE")
            throw new ArgumentException("El conductor no está disponible.");
        return driver;
    }

    private async Task<Vehicle> ValidateVehicleAsync(int vehicleId, int entrepreneurId)
    {
        var vehicle = await vehicleRepository.FindByIdAsync(vehicleId);
        if (vehicle == null)
            throw new ArgumentException("El ID del vehículo no fue encontrado.");
        if (vehicle.EntrepreneurId != entrepreneurId)
            throw new ArgumentException("El vehículo no pertenece al empresario indicado.");
        if (vehicle.State != "AVAILABLE")
            throw new ArgumentException("El vehículo no está disponible.");
        return vehicle;
    }

    private static bool IsValidDecimal10_2(decimal value)
    {
        if (value <= 0)
            return false;
        if (value > 99999999.99m)
            return false;
        return value == decimal.Round(value, 2);
    }
}