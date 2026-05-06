using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserRoleRemovedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId,
    Guid           RoleId) : IDomainEvent;