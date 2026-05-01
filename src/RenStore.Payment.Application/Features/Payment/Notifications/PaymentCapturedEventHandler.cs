using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class PaymentCapturedEventHandler(
    IPaymentProjection paymentProjection)
    : INotificationHandler<DomainEventNotification<PaymentCapturedEvent>>
{
    public async Task Handle(
        DomainEventNotification<PaymentCapturedEvent> notification,
        CancellationToken                             cancellationToken)
    {
        var e = notification.DomainEvent;

        await paymentProjection.SetCapturedAsync(
            now:               e.OccurredAt,
            paymentId:         e.PaymentId,
            cancellationToken: cancellationToken);
    }
}