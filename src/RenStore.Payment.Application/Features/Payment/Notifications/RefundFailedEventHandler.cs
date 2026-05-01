using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class RefundFailedEventHandler(
    IRefundProjection refundProjection)
    : INotificationHandler<DomainEventNotification<RefundFailedEvent>>
{
    public async Task Handle(
        DomainEventNotification<RefundFailedEvent> notification,
        CancellationToken                          cancellationToken)
    {
        var e = notification.DomainEvent;

        await refundProjection.MarkAsFailedAsync(
            now:               e.OccurredAt,
            refundId:          e.RefundId,
            reason:            e.Reason,
            cancellationToken: cancellationToken);
    }
}