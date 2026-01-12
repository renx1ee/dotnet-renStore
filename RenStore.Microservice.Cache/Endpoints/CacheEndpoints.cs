using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using RenStore.Microservice.Cache.DTOs;
using RenStore.Microservice.Cache.Services;

namespace RenStore.Microservice.Cache.Endpoints;

public static class CacheEndpoints
{
    public static IEndpointRouteBuilder MapCacheEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("api/cache/");
        
        api.MapPost("/memory", SetMemoryCache);
        api.MapGet("/memory/{key}", GetMemoryCache);
        api.MapDelete("/memory/{key}", DeleteMemoryCache);

        api.MapPost("/distributed", SetDistributedCache);
        api.MapGet("/distributed/{key}", GetDistributedCache);
        api.MapDelete("/distributed/{key}", DeleteDistributedCache);
        
        return app;
    }
    #region Memory Cache Endpoints
    private static async Task<IResult> SetMemoryCache(
        [FromBody] SetCacheRequest request,
        MemoryCacheService serviceBase)
    {
        await serviceBase.SetCacheAsync(
            key: request.key, 
            data: request.value, 
            seconds: request.seconds, 
            cancellationToken: CancellationToken.None);
        
        return Results.NoContent();
    }
    
    private static async Task<IResult> GetMemoryCache(
        string key,
        MemoryCacheService serviceBase)
    {
        var result = await serviceBase.GetCacheAsync(
            key: key, 
            cancellationToken: CancellationToken.None);
        
        if (string.IsNullOrEmpty(result))
            return Results.NotFound();
        
        return Results.Ok(result);
    }
    
    private static async Task<IResult> DeleteMemoryCache(
        string key,
        MemoryCacheService serviceBase)
    {
        await serviceBase.DeleteCacheAsync(key, CancellationToken.None);
        return Results.NoContent();
    }
    #endregion
    #region Distibuted Cache Endpoints
    private static async Task<IResult> SetDistributedCache(
        [FromBody] SetCacheRequest request,
        DistributedCacheService serviceBase)
    {
        await serviceBase.SetCacheAsync(
            key: request.key, 
            data: request.value,
            seconds: request.seconds, 
            cancellationToken: CancellationToken.None);
        
        return Results.NoContent();
    }

    private static async Task<IResult> GetDistributedCache(
        string key,
        DistributedCacheService serviceBase)
    {
        var result = await serviceBase.GetCacheAsync(
            key: key, 
            CancellationToken.None);

        if (result is null)
            return Results.NotFound();

        return Results.Ok(result);
    }

    private static async Task<IResult> DeleteDistributedCache(
        string key,
        DistributedCacheService serviceBase)
    {
        await serviceBase.DeleteCacheAsync(key, CancellationToken.None);
        return Results.NoContent();
    }
    #endregion
}