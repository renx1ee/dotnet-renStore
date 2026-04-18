using MediatR;
using RenStore.Inventory.Application.Abstractions;
using RenStore.Inventory.Domain.Aggregates.Reservation;

namespace RenStore.Inventory.Persistence.Write.Repositories;

internal sealed class ReservationRepository
    :  RenStore.Inventory.Domain.Interfaces.Repository.IReservationRepository
{
    private readonly IEventStore _eventStore;
    
    public ReservationRepository(
        IEventStore eventStore)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    public async Task<VariantReservation?> GetAsync(
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        if (reservationId == Guid.Empty)
            throw new InvalidOperationException(nameof(reservationId));

        var events = await _eventStore.LoadAsync(
            aggregateId: reservationId,
            cancellationToken: cancellationToken);

        if (events.Count == 0) return null;
        
        return VariantReservation.Rehydrate(events);
    }

    public async Task SaveAsync(
        VariantReservation reservation,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(reservation);

        var uncommittedEvents = reservation.GetUncommittedEvents();
        
        if(uncommittedEvents.Count == 0) return;

        await _eventStore.AppendAsync(
            aggregateId: reservation.Id,
            expectedVersion: reservation.Version,
            events: uncommittedEvents.ToList(),
            cancellationToken: cancellationToken);
    }
}