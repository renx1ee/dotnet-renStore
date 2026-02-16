namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;

public record PriceDeleted(
    DateTimeOffset OccurredAt,
    Guid PriceId);