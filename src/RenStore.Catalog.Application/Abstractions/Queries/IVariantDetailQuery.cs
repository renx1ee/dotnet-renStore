namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IVariantDetailQuery
{
    Task<IReadOnlyList<VariantDetailReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductDetailSortBy sortBy = ProductDetailSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);

    Task<VariantDetailReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<VariantDetailReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<VariantDetailReadModel>> FindByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken,
        ProductDetailSortBy sortBy = ProductDetailSortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);
}