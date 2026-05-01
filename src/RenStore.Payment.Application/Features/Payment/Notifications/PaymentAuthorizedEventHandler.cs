using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class PaymentAuthorizedEventHandler(
    IPaymentProjection        paymentProjection,
    IPaymentAttemptProjection attemptProjection)
    : INotificationHandler<DomainEventNotification<PaymentAuthorizedEvent>>
{
    public async Task Handle(
        DomainEventNotification<PaymentAuthorizedEvent> notification,
        CancellationToken                               cancellationToken)
    {
        var e = notification.DomainEvent;

        await paymentProjection.SetAuthorizedAsync(
            now:               e.OccurredAt,
            paymentId:         e.PaymentId,
            externalPaymentId: e.ExternalPaymentId,
            cancellationToken: cancellationToken);

        await attemptProjection.MarkAsSuccessfulAsync(
            now:              e.OccurredAt,
            attemptId:        e.AttemptId,
            externalAuthCode: e.ExternalAuthCode,
            cancellationToken: cancellationToken);
    }
}