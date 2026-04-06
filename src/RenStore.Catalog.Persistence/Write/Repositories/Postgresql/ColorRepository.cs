using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Entities;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

internal sealed class ColorRepository 
    : RenStore.Catalog.Domain.Interfaces.Repository.IColorRepository
{
    private readonly CatalogDbContext _context;
    
    public ColorRepository(
        CatalogDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task AddAsync(
        Color product,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(product);
        
        await _context.Colors.AddAsync(product, cancellationToken);
    }
    
    public async Task<Color?> GetAsync(
        int colorId,
        CancellationToken cancellationToken)
    {
        if (colorId <= 0)
            throw new ArgumentOutOfRangeException(nameof(colorId));
        
        return await _context.Colors
            .FirstOrDefaultAsync(x => 
                x.Id == colorId, 
                cancellationToken);
    }

    public async Task<bool> IsExists(
        int colorId,
        CancellationToken cancellationToken)
    {
        return await _context.Colors.AnyAsync(x => 
            x.Id == colorId, 
            cancellationToken);
    }
}