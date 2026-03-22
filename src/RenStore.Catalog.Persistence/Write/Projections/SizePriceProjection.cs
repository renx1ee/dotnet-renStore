using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class SizePriceProjection
    : RenStore.Catalog.Application.Abstractions.Projections.ISizePriceProjection
{
    private readonly CatalogDbContext _context;
    
    public SizePriceProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        PriceHistoryReadModel price,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(price);

        var prices = await _context.Prices
            .FirstOrDefaultAsync(x => 
                x.SizeId == price.SizeId && x.IsActive, 
                cancellationToken);
        
        if(prices is not null)
            prices.IsActive = false;

        await _context.Prices.AddAsync(price, cancellationToken);

        return price.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<PriceHistoryReadModel> prices,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(prices);

        var pricesList = prices as IList<PriceHistoryReadModel> ?? prices.ToList();

        if (pricesList.Count == 0) return;
        
        await _context.Prices.AddRangeAsync(pricesList, cancellationToken);
    }
    
    public void Remove(PriceHistoryReadModel price)
    {
        ArgumentNullException.ThrowIfNull(price);

        _context.Prices.Remove(price);
    }
    
    public void RemoveRange(IReadOnlyCollection<PriceHistoryReadModel> prices)
    {
        ArgumentNullException.ThrowIfNull(prices);

        _context.Prices.RemoveRange(prices);
    }
}