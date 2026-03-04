namespace RenStore.Catalog.Domain.ReadModels;

public class CategoryReadModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public string NormalizedName { get; set; }
    public string NameRu { get; set; } 
    public string NormalizedNameRu { get; set; } 
    public string? Description { get; set; }
    public bool IsActive { get; set; } 
    public bool IsDeleted { get; set; }
    public DateTimeOffset CreatedAt { get; set; } 
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}