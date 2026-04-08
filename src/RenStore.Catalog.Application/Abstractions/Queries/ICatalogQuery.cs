using RenStore.Catalog.Application.Filters;

namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface ICatalogQuery
{
    Task<IReadOnlyList<CatalogReadModel>> SearchAsync(
        CatalogSearchFilter filter,
        CancellationToken cancellationToken = default);
}