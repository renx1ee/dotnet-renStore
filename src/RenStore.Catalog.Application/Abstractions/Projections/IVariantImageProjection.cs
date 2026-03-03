using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantImageProjection
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