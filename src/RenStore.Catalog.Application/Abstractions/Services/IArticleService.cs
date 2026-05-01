namespace RenStore.Catalog.Application.Abstractions.Services;

public interface IArticleService
{
    Task<long> GenerateAsync(CancellationToken cancellationToken);
}