using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Reservation.Events;

namespace RenStore.Inventory.Application.Features.Reservation.Notifications;

internal sealed class VariantReservationExpiredEventHandler
    : INotificationHandler<DomainEventNotification<VariantReservationExpiredEvent>>
{
    private readonly IReservationProjection _reservationProjection;
    
    public VariantReservationExpiredEventHandler(
        IReservationProjection reservationProjection)
    {
        _reservationProjection = reservationProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantReservationExpiredEvent> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await _reservationProjection.MarkAsExpiredAsync(
            now:           e.OccurredAt,
            reservationId: e.EventId,
            cancellationToken: cancellationToken);
    }
}