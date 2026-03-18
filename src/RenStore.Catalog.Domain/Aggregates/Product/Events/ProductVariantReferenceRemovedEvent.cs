using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Product.Events;

public sealed record ProductVariantReferenceRemovedEvent(
    Guid EventId,
    Guid ProductId,
    Guid VariantId,
    DateTimeOffset OccurredAt)
    : IDomainEvent;