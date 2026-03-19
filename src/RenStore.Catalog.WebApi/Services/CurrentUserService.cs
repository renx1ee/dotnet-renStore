using System.Security.Claims;
using RenStore.Catalog.Application.Abstractions;

namespace RenStore.Catalog.WebApi.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => Guid.Parse(
        _httpContextAccessor.HttpContext!.User
            .FindFirst(ClaimTypes.NameIdentifier)!.Value);       
    
    public string Role => 
        _httpContextAccessor.HttpContext!.User
            .FindFirst(ClaimTypes.Role)!.Value;


    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}