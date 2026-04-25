namespace RenStore.Catalog.Application.Abstractions.Repository;

public interface IProductVariantSizeRepository
{
    Task<IReadOnlyList<VariantSizeReadModel>> GetByVariantIdAsync(
        Guid variantId,
        CancellationToken cancellationToken);
}