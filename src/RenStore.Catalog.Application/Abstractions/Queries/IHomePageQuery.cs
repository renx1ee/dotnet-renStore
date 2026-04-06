using RenStore.Catalog.Application.Filters;

namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IHomePageQuery
{
    Task<IReadOnlyList<CatalogHomeItemReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageSize = 25,
        bool descending = false);

    Task<IReadOnlyList<CatalogHomeItemReadModel>> FindAsync(
        CatalogFilter filter,
        CancellationToken cancellationToken = default);
}