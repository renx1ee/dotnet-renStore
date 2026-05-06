using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderTrackingNumberAssignedEventHandler(
    IDeliveryOrderProjection deliveryOrderProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderTrackingNumberAssignedEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderTrackingNumberAssignedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        
        await deliveryOrderProjection.SetTrackingNumberAsync(
            trackingNumber:    e.TrackingNumber, 
            deliveryOrderId:   e.DeliveryOrderId,
            cancellationToken: cancellationToken);
    }
}