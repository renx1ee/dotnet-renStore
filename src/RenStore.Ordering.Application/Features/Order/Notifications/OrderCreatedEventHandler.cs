/*using MediatR;
using RenStore.Order.Application.Abstractions.Projections;
using RenStore.Order.Application.Common;
using RenStore.Order.Domain.Aggregates.Order.Events;
using RenStore.Order.Domain.ReadModels;

namespace RenStore.Order.Application.Features.Order.Notifications;

internal sealed class OrderCreatedEventHandler(
    IOrderProjection orderProjection)
    : INotificationHandler<DomainEventNotification<OrderCreatedEvent>>
{
    public async Task Handle(
        DomainEventNotification<OrderCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        
        var order = new OrderReadModel()
        {
            Id = e.OrderId,
            CustomerId = e.CustomerId,
            Status = ,
            ShippingAddress = e.ShippingAddress,
            TotalAmount = ,
            CreatedAt = e.OccurredAt
        };

        await orderProjection.AddAsync(order, cancellationToken);
    }
}*/