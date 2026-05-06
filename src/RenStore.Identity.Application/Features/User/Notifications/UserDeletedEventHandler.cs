using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserDeletedEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserDeletedEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserDeletedEvent> notification,
        CancellationToken                         cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.SetDeletedAsync(e.OccurredAt, e.UserId, cancellationToken);
    }
}