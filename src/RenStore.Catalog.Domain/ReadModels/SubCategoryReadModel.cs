namespace RenStore.Catalog.Domain.ReadModels;

public sealed class SubCategoryReadModel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public string NameRu { get; private set; }
    public string NormalizedNameRu { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public Guid CategoryId { get; private set; }
}