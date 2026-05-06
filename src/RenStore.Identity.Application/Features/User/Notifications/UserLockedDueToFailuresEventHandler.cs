using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserLockedDueToFailuresEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserLockedDueToFailuresEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserLockedDueToFailuresEvent> notification,
        CancellationToken                                     cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.SetLockoutAsync(e.OccurredAt, e.UserId, e.LockoutEnd, cancellationToken);
    }
}