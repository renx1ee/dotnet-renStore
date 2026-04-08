namespace RenStore.Catalog.Domain.ReadModels;

public sealed class GetCategoryWithSubCategoryReadModel
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public string NameRu { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public IList<GetSubCategoryReadModel> SubCategories { get; set; } = new List<GetSubCategoryReadModel>();
};