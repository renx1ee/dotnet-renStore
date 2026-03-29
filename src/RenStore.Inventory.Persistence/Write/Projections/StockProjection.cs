using Microsoft.EntityFrameworkCore;
using RenStore.Inventory.Domain.Enums;
using RenStore.Inventory.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Inventory.Persistence.Write.Projections;

internal sealed class StockProjection
    : RenStore.Inventory.Application.Abstractions.Projections.IStockProjection
{
    private readonly InventoryDbContext _context;

    public StockProjection(
        InventoryDbContext context)
    {
        _context = context;
    }
    
    public async Task SaveChangesAsync(
        CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Guid> AddAsync(
        VariantStockReadModel stock,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stock);

        await _context.Stocks.AddAsync(stock, cancellationToken);

        return stock.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantStockReadModel> stocks,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(stocks);

        var stocksList = stocks as IList<VariantStockReadModel> ?? stocks.ToList();

        if (stocksList.Count == 0) return;

        await _context.Stocks.AddRangeAsync(stocksList, cancellationToken);
    }

    public async Task AddToStockAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken)
    {
        var stock = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);

        stock.InStock += count;
        stock.UpdatedAt = now;
    }
    
    public async Task StockWriteOffAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        WriteOffReason reason,
        CancellationToken cancellationToken)
    {
        var stock = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        stock.Reason = reason; 
        stock.InStock -= count;
        stock.UpdatedAt = now;
    }
    
    public async Task SellAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken)
    {
        var stock = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        stock.InStock -= count;
        stock.Sales += count;
        stock.UpdatedAt = now;
    }
    
    public async Task ReturnSaleSeleAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken)
    {
        var stock = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        stock.InStock += count;
        stock.Sales -= count;
        stock.UpdatedAt = now;
    }
    
    public async Task SetStockAsync(
        DateTimeOffset now,
        Guid reservationId,
        int count,
        CancellationToken cancellationToken)
    {
        var stock = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);
        
        stock.InStock = count;
        stock.UpdatedAt = now;
    }
    
    public async Task SoftDelete(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var stock = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);

        stock.IsDeleted = true;
        stock.UpdatedAt = now;
        stock.DeletedAt = now;
    }
    
    public async Task Restore(
        DateTimeOffset now,
        Guid reservationId,
        CancellationToken cancellationToken)
    {
        var stock = await GetAsync(
            id: reservationId,
            cancellationToken: cancellationToken);

        stock.IsDeleted = false;
        stock.UpdatedAt = now;
        stock.DeletedAt = null;
    }
    
    public void Remove(
        VariantStockReadModel stock)
    {
        ArgumentNullException.ThrowIfNull(stock);

        _context.Stocks.Remove(stock);
    }
    
    public void RemoveRange(
        IReadOnlyCollection<VariantStockReadModel> stock)
    {
        ArgumentNullException.ThrowIfNull(stock);

        _context.Stocks.RemoveRange(stock);
    }
    
    private async Task<VariantStockReadModel> GetAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await _context.Stocks
            .FirstOrDefaultAsync(x =>
                    x.Id == id,
                cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(
                name: typeof(VariantStockReadModel), 
                id);
        }

        return result;
    }
}