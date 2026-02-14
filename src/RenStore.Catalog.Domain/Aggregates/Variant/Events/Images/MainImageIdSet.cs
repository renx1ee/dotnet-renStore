namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Images;

public record MainImageIdSet(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ImageId);