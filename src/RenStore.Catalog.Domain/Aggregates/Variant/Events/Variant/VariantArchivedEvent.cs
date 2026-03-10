using RenStore.SharedKernal.Domain.Common;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;

public record VariantArchivedEvent(
    Guid EventId,
    DateTimeOffset OccurredAt,
    Guid VariantId)
    : IDomainEvent;