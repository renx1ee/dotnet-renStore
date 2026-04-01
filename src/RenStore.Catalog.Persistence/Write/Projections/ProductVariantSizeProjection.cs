using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class ProductVariantSizeProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IProductVariantSizeProjection
{
    private readonly CatalogDbContext _context;
    
    public ProductVariantSizeProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        VariantSizeReadModel variant,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variant);

        await _context.Sizes.AddAsync(variant, cancellationToken);

        return variant.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantSizeReadModel> variants,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variants);

        var variantsList = variants as IList<VariantSizeReadModel> ?? variants.ToList();

        if (variantsList.Count == 0) return;
        
        await _context.Sizes.AddRangeAsync(variantsList, cancellationToken);
    }
    
    public async Task SoftDeleteAsync(
        Guid variantId,
        Guid sizeId,
        DateTimeOffset removedAt,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        ValidateSizeId(sizeId);
        
        var size = await GetSizeAsync(
            sizeId: sizeId,
            variantId: variantId,
            cancellationToken: cancellationToken);

        size.IsDeleted = true;
        size.DeletedAt = removedAt;
        size.UpdatedAt = removedAt;
    }
    
    public async Task RestoreAsync(
        Guid variantId,
        Guid sizeId,
        DateTimeOffset restoredAt,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        ValidateSizeId(sizeId);
        
        var size = await GetSizeAsync(
            sizeId: sizeId,
            variantId: variantId,
            cancellationToken: cancellationToken);
        
        size.IsDeleted = false;
        size.DeletedAt = null;
        size.UpdatedAt = restoredAt;
    }
    
    public void Remove(VariantSizeReadModel variant)
    {
        ArgumentNullException.ThrowIfNull(variant);

        _context.Sizes.Remove(variant);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantSizeReadModel> variants)
    {
        ArgumentNullException.ThrowIfNull(variants);

        _context.Sizes.RemoveRange(variants);
    }
    
    private async Task<VariantSizeReadModel> GetSizeAsync(
        Guid sizeId,
        Guid variantId,
        CancellationToken cancellationToken)
    {
        var view = await _context.Sizes
            .FirstOrDefaultAsync(x =>
                    x.VariantId == variantId &&
                    x.Id == sizeId, 
                cancellationToken);
        
        if (view is null)
        {
            throw new NotFoundException(
                name: typeof(VariantSizeReadModel),
                sizeId);
        }

        return view;
    }

    private static void ValidateProductVariantId(Guid variantId)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));
    }
    
    private static void ValidateSizeId(Guid sizeId)
    {
        if (sizeId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(sizeId));
    }
}