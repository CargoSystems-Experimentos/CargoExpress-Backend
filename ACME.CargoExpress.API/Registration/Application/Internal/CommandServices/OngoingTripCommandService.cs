using ACME.CargoExpress.API.Registration.Domain.Model.Commands;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Registration.Domain.Repositories;
using ACME.CargoExpress.API.Registration.Domain.Services;
using ACME.CargoExpress.API.Shared.Domain.Repositories;
namespace ACME.CargoExpress.API.Registration.Application.Internal.CommandServices;

public class OngoingTripCommandService(IOngoingTripRepository ongoingTripRepository, ITripRepository tripRepository, IUnitOfWork unitOfWork)
    :IOngoingTripCommandService
{
    public async Task<OngoingTrip?> Handle(CreateOngoingTripCommand command)
    {
        var trip = await tripRepository.FindByIdAsync(command.TripId);
        if (trip == null)
        {
            throw new ArgumentException("TripId not found.");
        }

        var existingExpense = await ongoingTripRepository.FindByTripIdAsync(command.TripId);
        if (existingExpense != null)
        {
            throw new InvalidOperationException("An OnGoingTrip with the same TripId already exists.");
        }

        var ongoingTrip = new OngoingTrip(command, trip);
        await ongoingTripRepository.AddAsync(ongoingTrip);
        await unitOfWork.CompleteAsync();
        return ongoingTrip;
    }
    
    public async Task<OngoingTrip?> Handle(UpdateOngoingTripCommand command)
    {
        var ongoingTrip = await ongoingTripRepository.FindByIdAsync(command.OngoingTripId);
        if (ongoingTrip == null)
        {
            return null;
        }
        // Update the ongoingTrip information
        ongoingTrip.Latitude = command.Latitude;
        ongoingTrip.Longitude = command.Longitude;
        ongoingTrip.Speed = command.Speed;
        ongoingTrip.Distance = command.Distance;
        ongoingTrip.State = command.State;
        
        await unitOfWork.CompleteAsync();
        return ongoingTrip;
    }
}