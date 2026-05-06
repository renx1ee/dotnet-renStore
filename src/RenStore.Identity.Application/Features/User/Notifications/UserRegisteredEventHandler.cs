using RenStore.Identity.Application.Abstractions.Projections;
using RenStore.Identity.Application.Common;
using RenStore.Identity.Domain.Aggregates.User.Events;
using RenStore.Identity.Domain.Enums;

namespace RenStore.Identity.Application.Features.User.Notifications;

internal sealed class UserRegisteredEventHandler(
    IUserProjection userProjection)
    : INotificationHandler<DomainEventNotification<UserRegisteredEvent>>
{
    public async Task Handle(
        DomainEventNotification<UserRegisteredEvent> notification,
        CancellationToken                            cancellationToken)
    {
        var e = notification.DomainEvent;

        await userProjection.AddAsync(new ApplicationUserReadModel
        {
            Id            = e.UserId,
            FirstName     = e.Name.FirstName,
            LastName      = e.Name.LastName,
            FullName      = e.Name.FullName,
            Email         = e.Email.Value,
            EmailConfirmed = false,
            PasswordHash  = e.PasswordHash,
            Status        = ApplicationUserStatus.UnderReview,
            CreatedAt     = e.OccurredAt
        }, cancellationToken);
    }
}