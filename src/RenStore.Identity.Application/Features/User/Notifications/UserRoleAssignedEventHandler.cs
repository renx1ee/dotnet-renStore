using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserRoleAssignedEventHandler(
    IUserRoleProjection userRoleProjection,
    IApplicationRoleQuery userQuery)
    : INotificationHandler<DomainEventNotification<UserRoleAssignedEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserRoleAssignedEvent> notification,
        CancellationToken                              cancellationToken)
    {
        var e = notification.DomainEvent;

        var role = await userQuery.FindByIdAsync(e.RoleId, cancellationToken);

        if (role is null) return;

        await userRoleProjection.AddAsync(
            e.UserId, e.RoleId, role.Name, cancellationToken);
    }
}