namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantAvailabilityUpdated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    bool IsAvailable);