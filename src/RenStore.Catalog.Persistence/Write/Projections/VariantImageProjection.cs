using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Persistence.Write.Projections;

public class VariantImageProjection
    : RenStore.Catalog.Application.Abstractions.Projections.IVariantImageProjection
{
    private readonly CatalogDbContext _context;
    
    public VariantImageProjection(CatalogDbContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task<Guid> AddAsync(
        VariantImage image,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(image);

        await _context.Images.AddAsync(image, cancellationToken);

        return image.Id;
    }
    
    public async Task AddRangeAsync(
        IReadOnlyCollection<VariantImage> images,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(images);

        var imagesList = images as IList<VariantImage> ?? images.ToList();
        
        if(imagesList.Count == 0) return;

        await _context.Images.AddRangeAsync(imagesList, cancellationToken);
    }

    public void Remove(VariantImage image)
    {
        ArgumentNullException.ThrowIfNull(image);

        _context.Images.Remove(image);
    }
    
    public void RemoveRange(IReadOnlyCollection<VariantImage> images)
    {
        ArgumentNullException.ThrowIfNull(images);

        _context.Images.RemoveRange(images);
    }
}