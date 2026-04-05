using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record ProductVariantDto(
    Guid VariantId,
    string Name,
    long Article,
    string Status,
    string Url,
    Guid MainImageId,
    SizeSystem SizeSystem,
    SizeType SizeType,
    int ColorId);