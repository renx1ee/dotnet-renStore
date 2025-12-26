namespace RenStore.Domain.Entities;

public class SubCategoryEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string NameRu { get; set; } = string.Empty;
    public string NormalizedNameRu { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int CategoryId { get; set; }
    public CategoryEntity? Category { get; set; }
}