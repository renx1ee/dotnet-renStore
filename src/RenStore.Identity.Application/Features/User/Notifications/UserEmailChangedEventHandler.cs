using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserEmailChangedEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserEmailChangedEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserEmailChangedEvent> notification,
        CancellationToken                              cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.SetEmailAsync(e.OccurredAt, e.UserId, e.Email.Value, cancellationToken);
    }
}