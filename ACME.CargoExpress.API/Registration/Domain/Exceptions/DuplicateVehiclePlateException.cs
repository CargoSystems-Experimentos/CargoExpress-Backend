namespace ACME.CargoExpress.API.Registration.Domain.Exceptions;

public class DuplicateVehiclePlateException(string plate)
    : Exception($"La placa '{plate}' del vehículo ya está registrada para este empresario.");
