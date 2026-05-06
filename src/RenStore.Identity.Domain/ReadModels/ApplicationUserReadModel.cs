using RenStore.Identity.Domain.Enums;

namespace RenStore.Identity.Domain.ReadModels;

public sealed class ApplicationUserReadModel
{
    public Guid                    Id                { get; set; }
    public string                  FirstName         { get; set; } = string.Empty;
    public string                  LastName          { get; set; } = string.Empty;
    public string                  FullName          { get; set; } = string.Empty;
    public string                  Email             { get; set; } = string.Empty;
    public bool                    EmailConfirmed    { get; set; }
    public string                  PasswordHash      { get; set; } = string.Empty;
    public string?                 Phone             { get; set; }
    public bool                    PhoneConfirmed    { get; set; }
    public int                     AccessFailedCount { get; set; }
    public DateTimeOffset?         LockoutEnd        { get; set; }
    public ApplicationUserStatus   Status            { get; set; }
    public DateTimeOffset          CreatedAt         { get; set; }
    public DateTimeOffset?         UpdatedAt         { get; set; }
    public DateTimeOffset?         DeletedAt         { get; set; }
    
    public ICollection<UserRoleReadModel> Roles { get; set; } = new List<UserRoleReadModel>();
}