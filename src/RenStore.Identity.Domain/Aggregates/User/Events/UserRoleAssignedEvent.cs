using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserRoleAssignedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId,
    Guid           RoleId) : IDomainEvent;