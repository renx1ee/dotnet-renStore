using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderAwaitingPickupEventHandler(
    IDeliveryOrderProjection    orderProjection,
    IDeliveryTrackingProjection trackingProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderAwaitingPickupEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderAwaitingPickupEvent> notification,
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await orderProjection.UpdateStatusAsync(
            e.OccurredAt, e.DeliveryOrderId,
            DeliveryStatus.AwaitingPickup, cancellationToken);

        await trackingProjection.AddAsync(new DeliveryTrackingReadModel
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = e.DeliveryOrderId,
            Status          = DeliveryStatus.AwaitingPickup,
            PickupPointId   = e.PickupPointId,
            OccurredAt      = e.OccurredAt
        }, cancellationToken);
    }
}