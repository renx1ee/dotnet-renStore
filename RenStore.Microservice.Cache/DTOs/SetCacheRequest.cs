namespace RenStore.Microservice.Cache.DTOs;

public record SetCacheRequest(string key, string value, uint seconds = 3600);