using MediatR;
using RenStore.Inventory.Application.Abstractions;
using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Reservation;
using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Inventory.Persistence.Write.Repositories;

public sealed class ReservationRepository
    :  RenStore.Inventory.Domain.Interfaces.Repository.IReservationRepository
{
    private readonly IEventStore _eventStore;
    private readonly IMediator _mediator;
    
    public ReservationRepository(
        IEventStore eventStore,
        IMediator mediator)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        _mediator = mediator     ?? throw new ArgumentNullException(nameof(mediator));
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

        foreach (var domainEvent in uncommittedEvents)
        {
            var notificationType = typeof(DomainEventNotification<>)
                .MakeGenericType(domainEvent.GetType());

            var notification = (IDomainEvent)Activator
                .CreateInstance(notificationType, domainEvent)!;

            await _mediator.Publish(notification, cancellationToken);
        }
    }
}