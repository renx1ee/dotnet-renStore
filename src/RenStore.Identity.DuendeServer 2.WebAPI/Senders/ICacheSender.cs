namespace RenStore.Identity.DuendeServer.WebAPI.Senders;

public interface ICacheSender
{
    Task SetCacheAsync(string key, string value, uint seconds);
    Task<string?> GetCacheAsync(string key);
}