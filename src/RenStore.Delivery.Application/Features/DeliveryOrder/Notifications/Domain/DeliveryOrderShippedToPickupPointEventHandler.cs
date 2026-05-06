using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderShippedToPickupPointEventHandler(
    IDeliveryOrderProjection    orderProjection,
    IDeliveryTrackingProjection trackingProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderShippedToPickupPointEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderShippedToPickupPointEvent> notification,
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await orderProjection.SetPickupPointAsync(
            e.OccurredAt, e.DeliveryOrderId,
            e.PickupPointId, cancellationToken);

        await trackingProjection.AddAsync(new DeliveryTrackingReadModel
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = e.DeliveryOrderId,
            Status          = DeliveryStatus.EnRouteToPickupPoint,
            PickupPointId   = e.PickupPointId,
            OccurredAt      = e.OccurredAt
        }, cancellationToken);
    }
}