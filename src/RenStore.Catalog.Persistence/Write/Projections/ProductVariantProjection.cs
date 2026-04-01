using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class ProductVariantProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IProductVariantProjection
{
    private readonly CatalogDbContext _context;
    
    public ProductVariantProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        ProductVariantReadModel variant,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variant);

        await _context.Variants.AddAsync(variant, cancellationToken);

        return variant.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<ProductVariantReadModel> variants,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(variants);

        var variantsList = variants as IList<ProductVariantReadModel> ?? variants.ToList();

        if (variantsList.Count == 0) return;
        
        await _context.Variants.AddRangeAsync(variantsList, cancellationToken);
    }
    
    public async Task PublishAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        
        var view = await GetVariantAsync(variantId, cancellationToken);

        view.UpdatedAt = now;
        view.Status = ProductVariantStatus.Published;
    }

    public async Task ArchiveAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        
        var view = await GetVariantAsync(variantId, cancellationToken);

        view.UpdatedAt = now;
        view.Status = ProductVariantStatus.Archived;
    }
    
    public async Task DraftAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        
        var view = await GetVariantAsync(variantId, cancellationToken);

        view.UpdatedAt = now;
        view.Status = ProductVariantStatus.Draft;
    }
    
    public async Task ChangeNameAsync(
        Guid variantId,
        string name,
        string normalizedName,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        
        var view = await GetVariantAsync(variantId, cancellationToken);

        view.UpdatedAt = now;
        view.Name = name;
        view.NormalizedName = normalizedName;
    }
    
    public async Task SetMainImageIdAsyncAsync(
        Guid variantId,
        Guid imageId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateImageId(imageId);
        ValidateProductVariantId(variantId);
        
        var view = await GetVariantAsync(variantId, cancellationToken);

        view.MainImageId = imageId;
        view.UpdatedAt = now;
    }
    
    public async Task SoftDeleteAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        
        var view = await GetVariantAsync(variantId, cancellationToken);

        view.UpdatedAt = now;
        view.DeletedAt = now;
        view.Status = ProductVariantStatus.Deleted;
    }
    
    public async Task RestoreAsync(
        Guid variantId,
        DateTimeOffset now,
        CancellationToken cancellationToken)
    {
        ValidateProductVariantId(variantId);
        
        var view = await GetVariantAsync(variantId, cancellationToken);

        view.DeletedAt = null;
        view.UpdatedAt = now;
        view.Status = ProductVariantStatus.Draft;
    }

    public void Remove(ProductVariantReadModel variant)
    {
        ArgumentNullException.ThrowIfNull(variant);

        _context.Variants.Remove(variant);
    }
    
    public void RemoveRange(IReadOnlyCollection<ProductVariantReadModel> variants)
    {
        ArgumentNullException.ThrowIfNull(variants);

        _context.Variants.RemoveRange(variants);
    }
    
    private async Task<ProductVariantReadModel> GetVariantAsync(
        Guid variantId,
        CancellationToken cancellationToken)
    {
        var view = await _context.Variants
            .FirstOrDefaultAsync(x => 
                x.Id == variantId, 
                cancellationToken);

        if (view is null)
        {
            throw new NotFoundException(
                name: typeof(ProductVariantReadModel),
                variantId);
        }

        return view;
    }

    private static void ValidateProductVariantId(Guid variantId)
    {
        if (variantId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(variantId));
    }
    
    private static void ValidateImageId(Guid imageId)
    {
        if (imageId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(imageId));
    }
}