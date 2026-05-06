using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserEmailConfirmedEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserEmailConfirmedEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserEmailConfirmedEvent> notification,
        CancellationToken                                cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.SetEmailConfirmedAsync(e.OccurredAt, e.UserId, cancellationToken);
    }
}