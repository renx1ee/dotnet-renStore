namespace RenStore.Catalog.Domain.DTOs.Product.FullPage;

public record ProductImageDto
(
    Guid Id,
    string StoragePath,
    bool IsMain,
    short SortOrder
);