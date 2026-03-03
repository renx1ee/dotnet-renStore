using RenStore.Catalog.Domain.Enums.Sorting;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IProductVariantQuery
{
    Task<IReadOnlyList<ProductVariantReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);

    Task<ProductVariantReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<ProductVariantReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<ProductVariantReadModel>> FindByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken,
        ProductVariantSortBy sortBy = ProductVariantSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null);
}