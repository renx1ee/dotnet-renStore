using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;
using RenStore.Payment.Domain.Enums;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class RefundInitiatedEventHandler(
    IRefundProjection refundProjection)
    : INotificationHandler<DomainEventNotification<RefundInitiatedEvent>>
{
    public async Task Handle(
        DomainEventNotification<RefundInitiatedEvent> notification,
        CancellationToken                             cancellationToken)
    {
        var e = notification.DomainEvent;

        await refundProjection.AddAsync(new RefundReadModel
        {
            Id        = e.RefundId,
            PaymentId = e.PaymentId,
            Amount    = e.Amount,
            Currency  = e.Currency,
            Reason    = e.Reason,
            Status    = RefundStatus.Pending,
            CreatedAt = e.OccurredAt
        }, cancellationToken);
    }
}