using RenStore.Inventory.Domain.Enums;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Application.Abstractions.Projections;

public interface IStockProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task<Guid> AddAsync(
        VariantStockReadModel stock,
        CancellationToken cancellationToken);

    Task AddRangeAsync(
        IReadOnlyCollection<VariantStockReadModel> stocks,
        CancellationToken cancellationToken);

    Task AddToStockAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken);

    Task StockWriteOffAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        WriteOffReason reason,
        CancellationToken cancellationToken);

    Task SellAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken);

    Task ReturnSaleSeleAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken);

    Task SetStockAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken);

    Task SoftDelete(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken);

    Task Restore(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken);

    void Remove(VariantStockReadModel stock);

    void RemoveRange(IReadOnlyCollection<VariantStockReadModel> stock);
}