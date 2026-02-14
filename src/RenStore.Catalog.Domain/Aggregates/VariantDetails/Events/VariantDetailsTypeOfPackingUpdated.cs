using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;

public record VariantDetailsTypeOfPackingUpdated(
    DateTimeOffset OccurredAt,
    TypeOfPackaging TypeOfPackaging,
    Guid VariantId);