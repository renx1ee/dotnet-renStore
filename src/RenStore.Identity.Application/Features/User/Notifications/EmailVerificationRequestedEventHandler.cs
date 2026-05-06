using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class EmailVerificationRequestedEventHandler(
    IEmailVerificationProjection verificationProjection,
    IApplicationUserQuery userQuery)
    : INotificationHandler<DomainEventNotification<EmailVerificationRequestedEvent>>
{
    public async Task Handle(
        DomainEventNotification<EmailVerificationRequestedEvent> notification,
        CancellationToken                                        cancellationToken)
    {
        var e = notification.DomainEvent;

        // Получаем текущий email пользователя из read model

        var user = await userQuery.FindByIdAsync(e.UserId, cancellationToken);

        if (user is null) return;

        await verificationProjection.AddAsync(new PendingEmailVerificationReadModel
        {
            Id        = Guid.NewGuid(),
            UserId    = e.UserId,
            Email     = user.Email,
            Token     = e.Token,
            IsUsed    = false,
            CreatedAt = e.OccurredAt,
            ExpiresAt = e.OccurredAt.AddHours(24)
        }, cancellationToken);

        // Здесь публикуем интеграционное событие для Notification-сервиса
        // чтобы он отправил email с токеном подтверждения
    }
}