using System.Security.Claims;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Application.Abstractions.Services;
using RenStore.Catalog.Application.Service;

namespace RenStore.Catalog.WebApi.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var claim = _httpContextAccessor.HttpContext!.User
                    .FindFirst(ClaimTypes.NameIdentifier);

            return Guid.NewGuid();
            
            return claim is null ? null : Guid.Parse(claim.Value);
        }
    }

    public string Role
    {
        get
        {
            return "Admin";
            
            return _httpContextAccessor.HttpContext!.User
                .FindFirst(ClaimTypes.Role)!.Value;
        }
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}