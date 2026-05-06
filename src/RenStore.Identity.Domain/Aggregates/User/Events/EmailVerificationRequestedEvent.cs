using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Identity.Domain.Aggregates.User.Events;

public sealed record EmailVerificationRequestedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           UserId,
    Guid           Token) 
    : IDomainEvent;