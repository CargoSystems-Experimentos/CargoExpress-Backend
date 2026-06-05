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
        var trip = await tripRepository.FindByIdAsync(command.TripId);
        if (trip == null)
        {
            throw new ArgumentException("TripId not found.");
        }

        var existingAlert = await alertRepository.FindByTitleAsync(command.Title);
        if (existingAlert != null)
        {
            throw new ArgumentException("Alert title is already registered.");
        }

        var alert = new Alert(command, trip);
        await alertRepository.AddAsync(alert);
        await unitOfWork.CompleteAsync();
        return alert;
    }
}