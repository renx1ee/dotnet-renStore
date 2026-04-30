using RenStore.Payment.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Payment.Domain.Aggregates.Payment.Events;

public sealed record PaymentCreatedEvent(
    Guid            EventId,
    DateTimeOffset  OccurredAt,
    Guid            PaymentId,
    Guid            OrderId,
    Guid            CustomerId,
    decimal         Amount,
    Currency        Currency,
    PaymentProvider Provider,
    PaymentMethod   PaymentMethod,
    DateTimeOffset  ExpiresAt,
    PaymentStatus   Status) 
    : IDomainEvent;