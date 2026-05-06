using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderShippedToSortingCenterEventHandler(
    IDeliveryOrderProjection    orderProjection,
    IDeliveryTrackingProjection trackingProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderShippedToSortingCenterEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderShippedToSortingCenterEvent> notification,
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await orderProjection.SetSortingCenterAsync(
            e.OccurredAt, e.DeliveryOrderId,
            currentSortingCenterId:     null,
            destinationSortingCenterId: e.DestinationSortingCenterId,
            cancellationToken);

        await trackingProjection.AddAsync(new DeliveryTrackingReadModel
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = e.DeliveryOrderId,
            Status          = DeliveryStatus.EnRouteToSortingCenter,
            SortingCenterId = e.DestinationSortingCenterId,
            OccurredAt      = e.OccurredAt
        }, cancellationToken);
    }
}