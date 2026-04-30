using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record PaymentAuthorizedEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           PaymentId,
    Guid           AttemptId,
    string         ExternalPaymentId,
    string?        ExternalAuthCode) 
    : IDomainEvent;