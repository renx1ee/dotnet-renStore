using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class PaymentFailedEventHandler(
    IPaymentProjection        paymentProjection,
    IPaymentAttemptProjection attemptProjection)
    : INotificationHandler<DomainEventNotification<PaymentFailedEvent>>
{
    public async Task Handle(
        DomainEventNotification<PaymentFailedEvent> notification,
        CancellationToken                           cancellationToken)
    {
        var e = notification.DomainEvent;

        await paymentProjection.SetFailedAsync(
            now:               e.OccurredAt,
            paymentId:         e.PaymentId,
            failureReason:     e.FailureReason,
            cancellationToken: cancellationToken);

        await attemptProjection.MarkAsFailedAsync(
            now:               e.OccurredAt,
            attemptId:         e.AttemptId,
            failureReason:     e.FailureReason,
            errorCode:         e.ProviderErrorCode,
            cancellationToken: cancellationToken);
    }
}