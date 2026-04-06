namespace RenStore.Catalog.Application.Service;

public interface IArticleService
{
    Task<long> GenerateAsync(CancellationToken cancellationToken);
}