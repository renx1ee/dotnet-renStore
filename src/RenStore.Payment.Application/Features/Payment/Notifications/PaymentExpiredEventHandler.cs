using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class PaymentExpiredEventHandler(
    IPaymentProjection paymentProjection)
    : INotificationHandler<DomainEventNotification<PaymentExpiredEvent>>
{
    public async Task Handle(
        DomainEventNotification<PaymentExpiredEvent> notification,
        CancellationToken                            cancellationToken)
    {
        var e = notification.DomainEvent;

        await paymentProjection.SetExpiredAsync(
            now:               e.OccurredAt,
            paymentId:         e.PaymentId,
            cancellationToken: cancellationToken);
    }
}