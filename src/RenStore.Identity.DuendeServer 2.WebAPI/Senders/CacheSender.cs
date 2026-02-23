using System.Text.Json;
using RenStore.Identity.DuendeServer.WebAPI.Data;
using RenStore.Identity.DuendeServer.WebAPI.DTOs;
using RenStore.Identity.DuendeServer.WebAPI.Service;

namespace RenStore.Identity.DuendeServer.WebAPI.Senders;

public class CacheSender : ICacheSender
{
    private readonly HttpClient httpClient;
    public CacheSender(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        httpClient.BaseAddress = new Uri(UrlConstants.CacheMicroserviceUrl);
    }
    public async Task SetCacheAsync(string key, string value, uint seconds)
    {
        var request = new SetCacheRequest(key, value, seconds);
        
        var response = await httpClient.PostAsJsonAsync(
            UrlConstants.DistributedUrl, request);
        
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
    }

    public async Task<string?> GetCacheAsync(string key)
    {
        try
        {
            using var response = 
                await httpClient.GetAsync(
                    $"{UrlConstants.DistributedUrl}/{key}");

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<string>(result);
        }
        catch (Exception ex)
        {
        }
        return string.Empty;
    }
}