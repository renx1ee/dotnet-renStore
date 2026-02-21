using RenStore.Catalog.Domain.Aggregates.Variant;

namespace RenStore.Catalog.Domain.Interfaces;

public interface IPriceHistoryRepository
{
    Task<Guid> AddAsync(
        PriceHistory history,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<PriceHistory> histories,
        CancellationToken cancellationToken);

    void Remove(PriceHistory history);

    void RemoveRange(IReadOnlyCollection<PriceHistory> histories);
}