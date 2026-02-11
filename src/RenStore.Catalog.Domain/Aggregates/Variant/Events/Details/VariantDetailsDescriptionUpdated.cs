namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsDescriptionUpdated(
    DateTimeOffset OccurredAt,
    string Description,
    Guid VariantId);