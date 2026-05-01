using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class PaymentCreatedEventHandler(
    IPaymentProjection paymentProjection)
    : INotificationHandler<DomainEventNotification<PaymentCreatedEvent>>
{
    public async Task Handle(
        DomainEventNotification<PaymentCreatedEvent> notification, 
        CancellationToken cancellationToken)
    {
        var e = notification.DomainEvent;
        
        await paymentProjection.AddAsync(new PaymentReadModel
        {
            Id             = e.PaymentId,
            OrderId        = e.OrderId,
            CustomerId     = e.CustomerId,
            Amount         = e.Amount,
            RefundedAmount = 0,
            Currency       = e.Currency,
            Status         = e.Status,
            Provider       = e.Provider,
            PaymentMethod  = e.PaymentMethod,
            ExpiresAt      = e.ExpiresAt,
            CreatedAt      = e.OccurredAt
        }, 
        cancellationToken);
    }
}