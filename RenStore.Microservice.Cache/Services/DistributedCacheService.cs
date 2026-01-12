using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace RenStore.Microservice.Cache.Services;

public class DistributedCacheService(IDistributedCache distributedCache) : ICacheServiceBase
{
    public async Task SetCacheAsync(string key, string data, uint seconds, CancellationToken cancellationToken)
    {
        /*data ??= DateTime.UtcNow.ToString("F");*/
        await distributedCache.SetStringAsync(
            key: key,
            value: data, 
            options: new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(seconds)
        }, 
        cancellationToken);
    }

    public async Task<string?> GetCacheAsync(string key, CancellationToken cancellationToken)
    {
        return await distributedCache.GetStringAsync(key, cancellationToken);
    }
    
    public async Task DeleteCacheAsync(string key, CancellationToken cancellationToken)
    {
        await distributedCache.RemoveAsync(key, cancellationToken);
    }
}