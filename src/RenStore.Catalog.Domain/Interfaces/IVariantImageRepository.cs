using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Domain.Interfaces;

public interface IVariantImageRepository
{
    Task<Guid> AddAsync(
        VariantImage image,
        CancellationToken cancellationToken);
    
    Task AddRangeAsync(
        IReadOnlyCollection<VariantImage> images,
        CancellationToken cancellationToken);
    
    void Remove(VariantImage image);
    
    void RemoveRange(IReadOnlyCollection<VariantImage> images);
}