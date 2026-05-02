using RenStore.Delivery.Application.Abstractions.Projections;
using RenStore.Delivery.Application.Common;
using RenStore.Delivery.Domain.Aggregates.DeliveryOrder.Events;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ReadModels;

namespace RenStore.Delivery.Application.Features.DeliveryOrder.Notifications.Domain;

internal sealed class DeliveryOrderCreatedEventHandler(
    IDeliveryOrderProjection  orderProjection,
    IDeliveryTrackingProjection trackingProjection)
    : INotificationHandler<DomainEventNotification<DeliveryOrderCreatedEvent>>
{
    public async Task Handle(
        DomainEventNotification<DeliveryOrderCreatedEvent> notification,
        CancellationToken                                  cancellationToken)
    {
        var e = notification.DomainEvent;

        await orderProjection.AddAsync(new DeliveryOrderReadModel
        {
            Id               = e.DeliveryOrderId,
            OrderId          = e.OrderId,
            DeliveryTariffId = e.DeliveryTariffId,
            Status           = DeliveryStatus.Placed,
            CreatedAt        = e.OccurredAt
        }, cancellationToken);

        await trackingProjection.AddAsync(new DeliveryTrackingReadModel
        {
            Id              = Guid.NewGuid(),
            DeliveryOrderId = e.DeliveryOrderId,
            Status          = DeliveryStatus.Placed,
            OccurredAt      = e.OccurredAt
        }, cancellationToken);

        await orderProjection.CommitAsync(cancellationToken);
    }
}