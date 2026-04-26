using RenStore.Inventory.Application.Common;
using RenStore.Inventory.Domain.Aggregates.Reservation.Events;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Application.Features.Reservation.Notifications;

internal sealed class VariantReservationCreatedEventHandler
    : INotificationHandler<DomainEventNotification<VariantReservationCreatedEvent>>
{
    private readonly IReservationProjection _reservationProjection;
    
    public VariantReservationCreatedEventHandler(
        IReservationProjection reservationProjection)
    {
        _reservationProjection = reservationProjection;
    }
    
    public async Task Handle(
        DomainEventNotification<VariantReservationCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await _reservationProjection.AddAsync(
            new VariantReservationReadModel()
            {
                Id        = e.ReservationId,
                Quantity  = e.Quantity,
                Status    = e.Status,
                CreatedAt = e.OccurredAt,
                ExpiresAt = e.ExpiresAt,
                VariantId = e.VariantId,
                SizeId    = e.SizeId,
                OrderId   = e.OrderId
            }, 
            cancellationToken);
    }
}