using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderArrivedAtSortingCenterEventHandler(
    IDeliveryOrderProjection    orderProjection,
    IDeliveryTrackingProjection trackingProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderArrivedAtSortingCenterEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderArrivedAtSortingCenterEvent> notification,
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;

        await orderProjection.SetSortingCenterAsync(
            e.OccurredAt, e.DeliveryOrderId,
            currentSortingCenterId:     e.SortingCenterId,
            destinationSortingCenterId: null,
            cancellationToken);

        await trackingProjection.AddAsync(new DeliveryTrackingReadModel
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = e.DeliveryOrderId,
            Status          = DeliveryStatus.ArrivedAtSortingCenter,
            SortingCenterId = e.SortingCenterId,
            OccurredAt      = e.OccurredAt
        }, cancellationToken);

        await orderProjection.CommitAsync(cancellationToken);
    }
}