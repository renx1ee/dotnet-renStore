using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.Role.Events;

public sealed record RoleCreatedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           RoleId,
    string         Name,
    string         NormalizedName,
    string         Description) 
    : IDomainEvent;