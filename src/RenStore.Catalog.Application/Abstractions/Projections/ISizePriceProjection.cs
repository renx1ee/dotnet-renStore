namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface ISizePriceProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Guid> AddAsync(
        PriceHistoryReadModel price,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<PriceHistoryReadModel> prices,
        CancellationToken cancellationToken);

    void Remove(PriceHistoryReadModel price);

    void RemoveRange(IReadOnlyCollection<PriceHistoryReadModel> prices);
}