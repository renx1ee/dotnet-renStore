using Microsoft.Extensions.Caching.Memory;

namespace RenStore.Microservice.Cache.Services;

public class MemoryCacheService(IMemoryCache memoryCache)
{
    public async Task SetCacheAsync(string key, string? data, uint seconds, CancellationToken cancellationToken)
    {
        data ??= DateTime.Now.ToString("F");
        memoryCache.Set(key, data, DateTime.Now.AddSeconds(seconds));
    }

    public async Task<string?> GetCacheAsync(string key, CancellationToken cancellationToken)
    {
        var isExist = memoryCache.TryGetValue(
            key: key, 
            value: out string? value);
        
        if (isExist) 
            return value;
        
        return string.Empty;
    }

    public async Task DeleteCacheAsync(string key, CancellationToken cancellationToken)
    {
        memoryCache.Remove(key);
    }
}