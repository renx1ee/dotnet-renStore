using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Reservation.Events;

namespace RenStore.Inventory.Application.Features.Reservation.Notifications;

internal sealed class VariantReservationSoftDeletedHandler
    : INotificationHandler<DomainEventNotification<VariantReservationSoftDeleted>>
{
    private readonly IReservationProjection _reservationProjection;
    
    public VariantReservationSoftDeletedHandler(
        IReservationProjection reservationProjection)
    {
        _reservationProjection = reservationProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantReservationSoftDeleted> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await _reservationProjection.SoftDelete(
            now:           e.OccurredAt,
            reservationId: e.ReservationId,
            cancellationToken: cancellationToken);
    }
}