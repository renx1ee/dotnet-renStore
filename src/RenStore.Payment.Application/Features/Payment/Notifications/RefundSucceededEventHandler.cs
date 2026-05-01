/*using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class RefundSucceededEventHandler(
    IRefundProjection  refundProjection,
    IPaymentProjection paymentProjection)
    : INotificationHandler<DomainEventNotification<RefundSucceededEvent>>
{
    public async Task Handle(
        DomainEventNotification<RefundSucceededEvent> notification,
        CancellationToken                             cancellationToken)
    {
        var e = notification.DomainEvent;

        await refundProjection.MarkAsSucceededAsync(
            now:              e.OccurredAt,
            refundId:         e.RefundId,
            externalRefundId: e.ExternalRefundId,
            cancellationToken: cancellationToken);

        await paymentProjection.UpdateRefundedAmountAsync(
            now:               e.OccurredAt,
            paymentId:         e.PaymentId,
            refundedAmount:    e.RefundedAmountTotal,
            status:            e.NewPaymentStatus,
            cancellationToken: cancellationToken);
    }
}*/