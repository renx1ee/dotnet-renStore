namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IPriceHistoryQuery
{
    Task<IReadOnlyList<PriceHistoryReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        PriceHistorySortBy sortBy = PriceHistorySortBy.Id,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false,
        bool? isActive = null);

    Task<PriceHistoryReadModel?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PriceHistoryReadModel> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<PriceHistoryReadModel>> FindBySizeIdAsync(
        Guid sizeId,
        CancellationToken cancellationToken,
        PriceHistorySortBy sortBy = PriceHistorySortBy.Id,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false,
        bool? isActive = null);

    Task<PriceHistoryReadModel?> FindActiveBySizeIdAsync(
        Guid sizeId,
        CancellationToken cancellationToken);
}