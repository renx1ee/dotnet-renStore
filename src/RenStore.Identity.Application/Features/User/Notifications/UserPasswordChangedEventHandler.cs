using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserPasswordChangedEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserPasswordChangedEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserPasswordChangedEvent> notification,
        CancellationToken                                 cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.SetPasswordHashAsync(e.OccurredAt, e.UserId, e.PasswordHash, cancellationToken);
    }
}