using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderDeliveredEventHandler(
    IDeliveryOrderProjection    orderProjection,
    IDeliveryTrackingProjection trackingProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderDeliveredEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderDeliveredEvent> notification,
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await orderProjection.SetDeliveredAsync(
            e.OccurredAt, e.DeliveryOrderId, cancellationToken);

        await trackingProjection.AddAsync(new DeliveryTrackingReadModel
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = e.DeliveryOrderId,
            Status          = DeliveryStatus.Delivered,
            OccurredAt      = e.OccurredAt
        }, cancellationToken);

        await orderProjection.CommitAsync(cancellationToken);
    }
}