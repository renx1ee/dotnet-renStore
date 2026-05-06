namespace RenStore.Identity.Domain.ReadModels;

public sealed class UserRoleReadModel
{
    public Guid   UserId    { get; set; }
    public Guid   RoleId    { get; set; }
    public string RoleName  { get; set; } = string.Empty;
}