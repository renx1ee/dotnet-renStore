using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserRestoredEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserRestoredEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserRestoredEvent> notification,
        CancellationToken                          cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.RestoreAsync(e.OccurredAt, e.UserId, cancellationToken);
    }
}