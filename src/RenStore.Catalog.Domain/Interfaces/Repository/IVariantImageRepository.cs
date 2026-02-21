using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IVariantImageRepository
{
    Task<VariantImage?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantImage image,
        CancellationToken cancellationToken);
    
    Task AddRangeAsync(
        IReadOnlyCollection<VariantImage> images,
        CancellationToken cancellationToken);
    
    void Remove(VariantImage image);
    
    void RemoveRange(IReadOnlyCollection<VariantImage> images);
}