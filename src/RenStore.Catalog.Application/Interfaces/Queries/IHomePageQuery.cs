using RenStore.Catalog.Domain.ReadModels;

namespace RenStore.Catalog.Application.Interfaces.Queries;

public interface IHomePageQuery
{
    Task<IReadOnlyList<CatalogHomeItemReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);
}