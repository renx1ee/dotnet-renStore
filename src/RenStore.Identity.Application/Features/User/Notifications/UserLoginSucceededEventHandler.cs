using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserLoginSucceededEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserLoginSucceededEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserLoginSucceededEvent> notification,
        CancellationToken                                cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.ResetAccessFailedCountAsync(e.OccurredAt, e.UserId, cancellationToken);
    }
}