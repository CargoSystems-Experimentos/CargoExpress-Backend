using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class AlertCommandService(IAlertRepository alertRepository, ITripRepository tripRepository, IUnitOfWork unitOfWork)
    : IAlertCommandService
{
    public async Task<Alert?> Handle(CreateAlertCommand command)
    {
        ValidateFields(command.Title, command.Type, command.Description, command.Date);

        var trip = await tripRepository.FindByIdAsync(command.TripId);
        if (trip is null)
            throw new ArgumentException("El viaje especificado no existe.");

        if (trip.State != "PROGRESS")
            throw new InvalidOperationException("No se puede crear una alerta si el viaje no está en progreso.");

        ValidateDate(command.Date, trip.LoadDate, trip.UnloadDate);

        var alert = new Alert(command, trip);
        await alertRepository.AddAsync(alert);
        await unitOfWork.CompleteAsync();
        return alert;
    }

    public async Task<Alert?> Handle(UpdateAlertCommand command)
    {
        ValidateFields(command.Title, command.Type, command.Description, command.Date);

        var alert = await alertRepository.FindByIdAsync(command.AlertId);
        if (alert is null)
            return null;

        var trip = await tripRepository.FindByIdAsync(alert.TripId);
        if (trip is null)
            throw new ArgumentException("El viaje especificado no existe.");

        ValidateDate(command.Date, trip.LoadDate, trip.UnloadDate);

        alert.Update(command);
        alertRepository.Update(alert);
        await unitOfWork.CompleteAsync();
        return alert;
    }

    private static void ValidateFields(string title, string type, string description, DateTime date)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título de la alerta es requerido.");
        if (title.Length > 60)
            throw new ArgumentException("El título no puede tener más de 60 caracteres.");

        if (string.IsNullOrWhiteSpace(type))
            throw new ArgumentException("El tipo de la alerta es requerido.");
        if (type.Length > 60)
            throw new ArgumentException("El tipo no puede tener más de 60 caracteres.");

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción de la alerta es requerida.");
        if (description.Length > 100)
            throw new ArgumentException("La descripción no puede tener más de 100 caracteres.");

        if (date == default)
            throw new ArgumentException("La fecha de la alerta es requerida.");
    }

    private static void ValidateDate(DateTime date, DateTime loadDate, DateTime unloadDate)
    {
        if (date <= loadDate)
            throw new ArgumentException("La fecha de la alerta no puede ser menor o igual a la fecha de inicio del viaje.");
        if (date >= unloadDate)
            throw new ArgumentException("La fecha de la alerta no puede ser mayor o igual a la fecha de fin del viaje.");
    }
}
