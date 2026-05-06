using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.Role.Events;

namespace RenStore.Identity.Application.Features.Role.Notifications;

internal sealed class RoleDeletedEventHandler(
    IRoleProjection roleProjection)
    : INotificationHandler<DomainEventNotification<RoleDeletedEvent>>
{
    public async Task Handle(
        DomainEventNotification<RoleDeletedEvent> notification,
        CancellationToken                         cancellationToken)
    {
        var e = notification.DomainEvent;

        await roleProjection.SetDeletedAsync(e.OccurredAt, e.RoleId, cancellationToken);
    }
}