namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IProductQuery
{
    Task<IReadOnlyList<ProductReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductSortBy sortBy = ProductSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isDeleted = null);

    Task<ProductReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<ProductReadModel?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ProductReadModel>> FindBySellerIdAsync(
        Guid sellerId,
        CancellationToken cancellationToken,
        ProductSortBy sortBy = ProductSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isDeleted = null);

    Task<IReadOnlyList<ProductReadModel>> FindBySubCategoryIdAsync(
        Guid subCategoryId,
        CancellationToken cancellationToken,
        ProductSortBy sortBy = ProductSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isDeleted = null);
}