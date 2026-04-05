using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record VariantSizeDto(
    Guid SizeId,
    LetterSize LetterSize,
    decimal? Number,
    SizeType Type,
    SizeSystem System);