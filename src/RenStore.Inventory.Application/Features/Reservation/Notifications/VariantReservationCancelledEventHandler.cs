using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Reservation.Events;

namespace RenStore.Inventory.Application.Features.Reservation.Notifications;

internal sealed class VariantReservationCancelledEventHandler
    : INotificationHandler<DomainEventNotification<VariantReservationCancelledEvent>>
{
    private readonly IReservationProjection _reservationProjection;
    
    public VariantReservationCancelledEventHandler(
        IReservationProjection reservationProjection)
    {
        _reservationProjection = reservationProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantReservationCancelledEvent> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        
        await _reservationProjection.MarkAsCancelAsync(
            now:           e.OccurredAt,
            reason:        e.CancelReason,
            reservationId: e.Id,
            cancellationToken: cancellationToken);
    }
}