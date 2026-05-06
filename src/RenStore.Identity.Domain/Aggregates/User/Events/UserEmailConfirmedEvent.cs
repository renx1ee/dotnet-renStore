using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record UserEmailConfirmedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId) 
    : IDomainEvent;