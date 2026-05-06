using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.Role.Events;

namespace RenStore.Identity.Application.Features.Role.Notifications;

internal sealed class RoleCreatedEventHandler(
    IRoleProjection roleProjection)
    : INotificationHandler<DomainEventNotification<RoleCreatedEvent>>
{
    public async Task Handle(
        DomainEventNotification<RoleCreatedEvent> notification,
        CancellationToken                         cancellationToken)
    {
        var e = notification.DomainEvent;

        await roleProjection.AddAsync(new RoleReadModel
        {
            Id             = e.RoleId,
            Name           = e.Name,
            NormalizedName = e.NormalizedName,
            Description    = e.Description,
            IsDeleted      = false,
            CreatedAt      = e.OccurredAt
        }, cancellationToken);
    }
}