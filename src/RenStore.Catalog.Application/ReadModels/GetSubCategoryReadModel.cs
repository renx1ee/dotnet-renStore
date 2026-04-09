namespace RenStore.Catalog.Application.ReadModels;

public sealed class GetSubCategoryReadModel
{
    public Guid SubCategoryId { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public string NameRu { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } 
}