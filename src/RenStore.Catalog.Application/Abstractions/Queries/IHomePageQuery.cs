namespace RenStore.Catalog.Application.Abstractions.Queries;

public interface IHomePageQuery
{
    Task<IReadOnlyList<CatalogHomeItemReadModel>> FindAllAsync(
        CancellationToken cancellationToken,
        uint page = 1,
        uint pageCount = 25,
        bool descending = false);
}