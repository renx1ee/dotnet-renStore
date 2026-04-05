namespace RenStore.Catalog.Domain.ReadModels.Product.FullPage;

public sealed record ProductVariantImageDto(
    Guid ImageId,
    string StoragePath,
    bool IsMain,
    int SortOrder,
    Guid VariantId
);