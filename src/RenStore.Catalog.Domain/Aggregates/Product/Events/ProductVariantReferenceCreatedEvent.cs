using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

public sealed record ProductVariantReferenceCreatedEvent(
    Guid EventId,
    Guid ProductId,
    Guid VariantId,
    DateTimeOffset OccurredAt)
    : IDomainEvent;