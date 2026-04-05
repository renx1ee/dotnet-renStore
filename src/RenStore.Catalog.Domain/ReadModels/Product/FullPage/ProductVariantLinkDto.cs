namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record ProductVariantLinkDto(
    Guid VariantId,
    string Url,
    Guid MainImageId,
    string StoragePath);