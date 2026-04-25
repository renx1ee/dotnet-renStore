using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Application.Abstractions.Repository;

namespace RenStore.Catalog.Persistence.Read.Repository;

internal sealed class ProductVariantSizeRepository : IProductVariantSizeRepository
{
    private readonly CatalogDbContext _context;
    
    public ProductVariantSizeRepository(
        CatalogDbContext context)
    {
        _context = context;
    }
    
    public async Task<IReadOnlyList<VariantSizeReadModel>> GetByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken)
    {
        return await _context.Sizes.Where(x => x.VariantId == variantId)
            .ToListAsync(cancellationToken);
    }
}