using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.Role.Events;

namespace RenStore.Identity.Application.Features.Role.Notifications;

internal sealed class RoleUpdatedEventHandler(
    IRoleProjection roleProjection)
    : INotificationHandler<DomainEventNotification<RoleUpdatedEvent>>
{
    public async Task Handle(
        DomainEventNotification<RoleUpdatedEvent> notification,
        CancellationToken                         cancellationToken)
    {
        var e = notification.DomainEvent;

        await roleProjection.UpdateAsync(
            e.OccurredAt, e.RoleId,
            e.Name, e.NormalizedName, e.Description,
            cancellationToken);
    }
}