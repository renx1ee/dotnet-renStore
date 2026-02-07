using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

public record VariantSizeCreated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId,
    int InStock,
    LetterSize LetterSize,
    SizeSystem SizeSystem,
    SizeType SizeType);