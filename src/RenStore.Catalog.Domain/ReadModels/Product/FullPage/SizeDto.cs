namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record SizeDto(
    VariantSizeDto Size,
    PriceHistoryDto History);