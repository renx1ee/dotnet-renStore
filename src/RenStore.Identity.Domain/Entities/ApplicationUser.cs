using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace RenStore.Identity.Domain.Entities;

public class ApplicationUser : IdentityUser, IUser<string>
{
    public override string? UserName { get; set; }
    public override string? NormalizedUserName { get; set; }
    public override string? Email { get; set; }
    public override bool EmailConfirmed { get; set; }
    public override string? PasswordHash { get; set; }
    public override bool PhoneNumberConfirmed { get; set; }
    public override DateTimeOffset? LockoutEnd { get; set; }
    public override bool LockoutEnabled { get; set; }
    public override int AccessFailedCount { get; set; }
    
    public string? Name { get; private set; }
    public override string? PhoneNumber { get; set; }
    public double? Balance { get; private set; } 
    public bool? IsActive { get; private set; } 
    public DateTimeOffset CreatedDate { get; private set; }

    public void AccountFreeze()
    {
    }
}