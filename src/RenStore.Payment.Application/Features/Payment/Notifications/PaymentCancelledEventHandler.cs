using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class PaymentCancelledEventHandler(
    IPaymentProjection paymentProjection)
    : INotificationHandler<DomainEventNotification<PaymentCancelledEvent>>
{
    public async Task Handle(
        DomainEventNotification<PaymentCancelledEvent> notification,
        CancellationToken                              cancellationToken)
    {
        var e = notification.DomainEvent;

        await paymentProjection.SetCancelledAsync(
            now:               e.OccurredAt,
            paymentId:         e.PaymentId,
            reason:            e.Reason,
            cancellationToken: cancellationToken);
    }
}
