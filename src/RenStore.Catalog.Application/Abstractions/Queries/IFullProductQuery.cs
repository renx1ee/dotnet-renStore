using RenStore.Catalog.Domain.ReadModels.Product.FullPage;

namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IFullProductQuery
{
    Task<FullProductPageDto?> FindFullAsync(
        Guid variantId,
        CancellationToken cancellationToken);
}