using System.Security.Claims;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.WebApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static UserRole GetRole(this ClaimsPrincipal user)
    {
        var role = user.FindFirstValue(ClaimTypes.Role);
        
        return role switch
        {
            "Admin" => UserRole.Admin,
            "Seller" => UserRole.Seller,
            "Buyer" => UserRole.Buyer,
            "Moderator" => UserRole.Moderator,
            "Support" => UserRole.Support,
            _ => UserRole.Guest
        };
    }

    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(id, out var userId) ? userId : Guid.Empty;
    }
}

