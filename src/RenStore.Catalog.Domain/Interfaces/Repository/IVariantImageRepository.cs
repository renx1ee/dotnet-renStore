using RenStore.Catalog.Domain.Aggregates.Media;

namespace RenStore.Catalog.Domain.Interfaces.Repository;

public interface IVariantImageRepository
{
    Task<VariantImage?> GetAsync(
        Guid id,
        CancellationToken cancellationToken);
    
    Task SaveAsync(
        VariantImage image,
        CancellationToken cancellationToken);
}