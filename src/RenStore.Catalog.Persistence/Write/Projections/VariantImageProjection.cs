using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.ReadModels;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Persistence.Write.Projections;

internal sealed class VariantImageProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IVariantImageProjection
{
    private readonly CatalogDbContext _context;
    
    public VariantImageProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task<Guid> AddAsync(
        VariantImageReadModel image,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(image);

        await _context.Images.AddAsync(image, cancellationToken);

        return image.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantImageReadModel> images,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(images);

        var imagesList = images as IList<VariantImageReadModel> ?? images.ToList();
        
        if(imagesList.Count == 0) return;

        await _context.Images.AddRangeAsync(imagesList, cancellationToken);
    }

    public async Task MarkAsMain(
        DateTimeOffset now,
        Guid imageId,
        CancellationToken cancellationToken)
    {
        ValidateImageId(imageId);

        var image = await GetImageAsync(
            imageId: imageId,
            cancellationToken: cancellationToken);

        image.IsMain = true;
        image.UpdatedAt = now;
    }
    
    public async Task UnmarkAsMain(
        DateTimeOffset now,
        Guid variantId,
        CancellationToken cancellationToken)
    {
        var images = await _context.Images
            .Where(x => x.VariantId == variantId)
            .ToListAsync(cancellationToken);

        foreach (var image in images)
        {
            image.IsMain = false;
            image.UpdatedAt = now;
        }
    }
    
    public async Task SoftDelete(
        DateTimeOffset now,
        Guid imageId,
        CancellationToken cancellationToken)
    {
        ValidateImageId(imageId);

        var image = await GetImageAsync(
            imageId: imageId,
            cancellationToken: cancellationToken);

        image.IsDeleted = true;
        image.IsMain = false;
        image.DeletedAt = now;
        image.UpdatedAt = now;
    }

    public void Remove(VariantImageReadModel image)
    {
        ArgumentNullException.ThrowIfNull(image);

        _context.Images.Remove(image);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantImageReadModel> images)
    {
        ArgumentNullException.ThrowIfNull(images);

        _context.Images.RemoveRange(images);
    }
    
    private async Task<VariantImageReadModel> GetImageAsync(
        Guid imageId,
        CancellationToken cancellationToken)
    {
        var view = await _context.Images
            .FirstOrDefaultAsync(x =>
                x.Id == imageId,
                cancellationToken: cancellationToken);
        
        if (view is null)
        {
            throw new NotFoundException(
                name: typeof(VariantImageReadModel),
                imageId);
        }

        return view;
    }
    
    private static void ValidateImageId(Guid imageId)
    {
        if (imageId == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(imageId));
    }
}