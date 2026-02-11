using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Details;

public record VariantDetailsTypeOfPackingUpdated(
    DateTimeOffset OccurredAt,
    TypeOfPackaging TypeOfPackaging,
    Guid VariantId);