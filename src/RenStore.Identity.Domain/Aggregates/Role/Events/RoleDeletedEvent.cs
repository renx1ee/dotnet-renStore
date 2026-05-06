using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.Role.Events;

public sealed record RoleDeletedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           RoleId) 
    : IDomainEvent;