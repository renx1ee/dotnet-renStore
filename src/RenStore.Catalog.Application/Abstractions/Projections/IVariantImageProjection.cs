using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Abstractions.Projections;

public interface IVariantImageProjection
{
    Task SaveChangesAsync(CancellationToken cancellationToken);
    
    Task<Guid> AddAsync(
        VariantImageReadModel image,
        CancellationToken cancellationToken);
    
    Task AddRangeAsync(
        IReadOnlyCollection<VariantImageReadModel> images,
        CancellationToken cancellationToken);
    
    void Remove(VariantImageReadModel image);
    
    void RemoveRange(IReadOnlyCollection<VariantImageReadModel> images);
}