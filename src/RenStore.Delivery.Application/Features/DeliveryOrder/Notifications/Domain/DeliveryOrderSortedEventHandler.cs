using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderSortedEventHandler(
    IDeliveryOrderProjection    orderProjection,
    IDeliveryTrackingProjection trackingProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderSortedEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderSortedEvent> notification,
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await orderProjection.UpdateStatusAsync(
            e.OccurredAt, e.DeliveryOrderId,
            DeliveryStatus.Sorted, cancellationToken);

        await trackingProjection.AddAsync(new DeliveryTrackingReadModel
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = e.DeliveryOrderId,
            Status          = DeliveryStatus.Sorted,
            SortingCenterId = e.SortingCenterId,
            OccurredAt      = e.OccurredAt
        }, cancellationToken);

        await orderProjection.CommitAsync(cancellationToken);
    }
}