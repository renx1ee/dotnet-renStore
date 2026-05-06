namespace RenStore.Identity.Domain.ReadModels;

public sealed class RoleReadModel
{
    public Guid            Id             { get; set; }
    public string          Name           { get; set; } = string.Empty;
    public string          NormalizedName { get; set; } = string.Empty;
    public string          Description    { get; set; } = string.Empty;
    public bool            IsDeleted      { get; set; }
    public DateTimeOffset  CreatedAt      { get; set; }
    public DateTimeOffset? UpdatedAt      { get; set; }
    public DateTimeOffset? DeletedAt      { get; set; }
}