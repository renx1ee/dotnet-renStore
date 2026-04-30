using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record PaymentAttemptCreatedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           PaymentId,
    Guid           AttemptId,
    int            AttemptNumber) 
    : IDomainEvent;