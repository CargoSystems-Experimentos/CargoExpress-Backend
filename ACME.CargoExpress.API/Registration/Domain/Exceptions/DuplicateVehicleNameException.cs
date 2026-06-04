namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class DuplicateVehicleNameException(string name)
    : Exception($"El nombre del vehículo '{name}' ya está registrado.");
