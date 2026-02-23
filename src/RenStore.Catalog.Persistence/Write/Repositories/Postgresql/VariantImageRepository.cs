using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Interfaces.Repository;

namespace RenStore.Catalog.Persistence.Write.Repositories.Postgresql;

public class VariantImageRepository
    : IVariantImageRepository
{
    private readonly CatalogDbContext _context;
    private readonly IEventStore _eventStore;
    
    public VariantImageRepository(
        CatalogDbContext context,
        IEventStore eventStore)
    {
        _context = context       ?? throw new ArgumentNullException(nameof(context));
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }
    
    public async Task<VariantImage?> GetAsync(
        Guid id, 
        CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentOutOfRangeException(nameof(id));
        
        var events = await _eventStore.LoadAsync(id, cancellationToken);

        if (!events.Any()) return null;
        
        return VariantImage.Rehydrate(events);
    }

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