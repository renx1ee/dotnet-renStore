using MediatR;
using RenStore.Payment.Application.Abstractions.Projection;
using RenStore.Payment.Application.Common;
using RenStore.Payment.Domain.Aggregates.Payment.Events;
using RenStore.Payment.Domain.ReadModels;

namespace RenStore.Payment.Application.Features.Payment.Notifications;

internal sealed class PaymentAttemptCreatedEventHandler(
    IPaymentAttemptProjection attemptProjection,
    IPaymentProjection        paymentProjection)
    : INotificationHandler<DomainEventNotification<PaymentAttemptCreatedEvent>>
{
    public async Task Handle(
        DomainEventNotification<PaymentAttemptCreatedEvent> notification,
        CancellationToken                                   cancellationToken)
    {
        var e = notification.DomainEvent;

        await attemptProjection.AddAsync(new PaymentAttemptReadModel
        {
            Id            = e.AttemptId,
            PaymentId     = e.PaymentId,
            AttemptNumber = e.AttemptNumber,
            IsSuccessful  = false,
            CreatedAt     = e.OccurredAt
        }, cancellationToken);

        // Обновляем LastAttemptId в PaymentReadModel
        await paymentProjection.SetLastAttemptIdAsync(
            now:              e.OccurredAt,
            paymentId:        e.PaymentId,
            lastAttemptId:    e.AttemptId,
            cancellationToken: cancellationToken);
    }
}