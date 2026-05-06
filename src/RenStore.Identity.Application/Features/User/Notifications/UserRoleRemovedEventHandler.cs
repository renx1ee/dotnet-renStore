using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserRoleRemovedEventHandler(
    IUserRoleProjection userRoleProjection)
    : INotificationHandler<DomainEventNotification<UserRoleRemovedEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserRoleRemovedEvent> notification,
        CancellationToken                             cancellationToken)
    {
        var e = notification.DomainEvent;

        await userRoleProjection.RemoveAsync(e.UserId, e.RoleId, cancellationToken);
    }
}