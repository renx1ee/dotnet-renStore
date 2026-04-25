using Microsoft.EntityFrameworkCore;
using RenStore.Inventory.Application.Abstractions.ReadRepository;
using RenStore.Inventory.Domain.ReadModels;

namespace RenStore.Inventory.Persistence.Read.Repository;

internal sealed class StockReadRepository : IStockReadRepository
{
    private readonly InventoryDbContext _context;

    public StockReadRepository(
        InventoryDbContext context)
    {
        _context = context;
    }
    
    public async Task<VariantStockReadModel?> GetAsync(
        Guid variantId,
        Guid sizeId,
        CancellationToken cancellationToken)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(x =>
                    x.VariantId == variantId &&
                    x.SizeId == sizeId,
                cancellationToken);
    }
    
    public async Task<VariantStockReadModel?> GetAsync(
        Guid stockId,
        CancellationToken cancellationToken)
    {
        return await _context.Stocks
            .FirstOrDefaultAsync(x => x.Id == stockId,
                cancellationToken);
    }
}