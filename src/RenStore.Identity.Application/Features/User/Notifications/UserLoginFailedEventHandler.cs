using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserLoginFailedEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserLoginFailedEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserLoginFailedEvent> notification,
        CancellationToken                             cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.IncrementAccessFailedCountAsync(e.OccurredAt, e.UserId, cancellationToken);
    }
}