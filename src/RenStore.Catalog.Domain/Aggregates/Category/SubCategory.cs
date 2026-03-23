namespace RenStore.Catalog.Domain.Aggregates.Category;

/// <summary>
/// Represents a sub category physical entity with lifecycle and invariants.
/// </summary>
public sealed class SubCategory
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public string NameRu { get; private set; }
    public string NormalizedNameRu { get; private set; }
    public string? Description { get; private set; }
    public Guid UpdatedById { get; private set; } 
    public string UpdatedByRole { get; private set; } 
    public bool IsActive { get; private set; } 
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public Guid CategoryId { get; private set; }
    
    private SubCategory() { }

    internal static SubCategory Create(
        Guid updatedById,
        string updatedByRole,
        Guid categoryId,
        Guid subCategoryId,
        DateTimeOffset now,
        string name,
        string normalizedName,
        string nameRu,
        string normalizedNameRu,
        bool isActive,
        string? description = null)
    {
        return new SubCategory()
        {
            Id = subCategoryId,
            UpdatedById = updatedById,
            UpdatedByRole = updatedByRole,
            CategoryId = categoryId,
            Name = name,
            NormalizedName = normalizedName,
            NameRu = nameRu,
            NormalizedNameRu = normalizedNameRu,
            IsDeleted = false,
            IsActive = isActive,
            CreatedAt = now
        };
    }
    
    internal void ChangeName(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string name,
        string normalizedName)
    {
        UpdatedById = updatedById;
        UpdatedByRole = updatedByRole;
        Name = name;
        NormalizedName = normalizedName;
        
        UpdatedAt = now;
    }
    
    internal void ChangeNameRu(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string nameRu,
        string normalizedNameRu)
    {
        UpdatedById = updatedById;
        UpdatedByRole = updatedByRole;
        NameRu = nameRu;
        NormalizedNameRu = normalizedNameRu;
        
        UpdatedAt = now;
    }
    
    internal void ChangeDescription(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now,
        string description)
    {
        UpdatedById = updatedById;
        UpdatedByRole = updatedByRole;
        Description = description;
        UpdatedAt = now;
    }
    
    internal void Activate(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        UpdatedById = updatedById;
        UpdatedByRole = updatedByRole;
        UpdatedAt = now;
        IsActive = true;
    }
    
    internal void Deactivate(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        UpdatedById = updatedById;
        UpdatedByRole = updatedByRole;
        UpdatedAt = now;
        IsActive = false;
    }
    
    internal void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        UpdatedById = updatedById;
        UpdatedByRole = updatedByRole;
        UpdatedAt = now;
        DeletedAt = now;
        IsActive = false;
        IsDeleted = true;
    }
    
    internal void Restore(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        UpdatedById = updatedById;
        UpdatedByRole = updatedByRole;
        UpdatedAt = now;
        DeletedAt = null;
        IsDeleted = false;
    }
}