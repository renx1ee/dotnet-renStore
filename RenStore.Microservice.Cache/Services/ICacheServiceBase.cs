namespace RenStore.Microservice.Cache.Services;

public interface ICacheServiceBase
{
    Task SetCacheAsync(string key, string? data, uint seconds, CancellationToken cancellationToken);
    Task<string?> GetCacheAsync(string key, CancellationToken cancellationToken);
    Task DeleteCacheAsync(string key, CancellationToken cancellationToken);
}