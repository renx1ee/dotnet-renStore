using System.Security.Claims;
using RenStore.Inventory.Application.Abstractions;

namespace RenStore.Inventory.WebApi.Services;

internal sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _contextAccessor;
    
    public CurrentUserService(
        IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Guid? UserId
    {
        get
        {
            var claim = _contextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.NameIdentifier);

            return claim is null ? null : Guid.Parse(claim.Value);
        }
    }

    public string Role
    {
        get
        {
            return _contextAccessor.HttpContext.User
                .FindFirst(ClaimTypes.Role)!.Value;
        }
    }

    public bool IsAuthenticated =>
        _contextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
}