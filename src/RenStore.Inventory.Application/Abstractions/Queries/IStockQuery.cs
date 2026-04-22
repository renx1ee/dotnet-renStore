using RenStore.Inventory.Application.ReadModels;
using RenStore.Inventory.Domain.Enums.Sorting;

namespace RenStore.Inventory.Application.Abstractions.Queries;

public interface IStockQuery
{
    Task<VariantStockDto?> FindByIdAsync(
        Guid id,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<VariantStockDto>> FindByVariantIdAsync(
        Guid variantId,
        StockSortBy sortBy = StockSortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);

    Task<VariantStockDto?> FindByVariantIdAsync(
        Guid variantId,
        Guid sizeId,
        bool? isDeleted = null,
        CancellationToken cancellationToken = default);
}