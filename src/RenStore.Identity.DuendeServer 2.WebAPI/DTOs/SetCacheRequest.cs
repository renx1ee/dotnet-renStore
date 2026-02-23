namespace RenStore.Identity.DuendeServer.WebAPI.DTOs;

public record SetCacheRequest(string key, string value, uint seconds = 360);