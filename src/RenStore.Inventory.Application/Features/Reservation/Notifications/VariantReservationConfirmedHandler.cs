using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Reservation.Events;

namespace RenStore.Inventory.Application.Features.Reservation.Notifications;

internal sealed class VariantReservationConfirmedHandler
    : INotificationHandler<DomainEventNotification<VariantReservationConfirmed>>
{
    private readonly IReservationProjection _reservationProjection;
    
    public VariantReservationConfirmedHandler(
        IReservationProjection reservationProjection)
    {
        _reservationProjection = reservationProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantReservationConfirmed> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await _reservationProjection.MarkAsConfirmedAsync(
            now: e.OccurredAt,
            reservationId: e.Id,
            cancellationToken: cancellationToken);
    }
}