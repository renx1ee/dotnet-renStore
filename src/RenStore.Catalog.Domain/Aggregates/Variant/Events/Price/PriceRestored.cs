namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;

public record PriceRestored(
    DateTimeOffset OccurredAt,
    Guid PriceId);