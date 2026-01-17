namespace RenStore.Catalog.Domain.DTOs.Product.FullPage;

public record ProductDto
(
    Guid Id,
    bool IsBlocked,
    decimal OverallRating,
    long SellerId,
    int CategoryId
);