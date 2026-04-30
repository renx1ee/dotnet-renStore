using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record RefundSucceededEvent(
    Guid           EventId,
    DateTimeOffset OccurredAt,
    Guid           PaymentId,
    Guid           RefundId,
    string         ExternalRefundId) 
    : IDomainEvent;