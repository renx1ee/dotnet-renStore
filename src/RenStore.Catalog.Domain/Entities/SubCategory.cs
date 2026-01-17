using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a sub category physical entity with lifecycle and invariants.
/// </summary>
public class SubCategory
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public string NameRu { get; private set; } = string.Empty;
    public string NormalizedNameRu { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    public int CategoryId { get; private set; }
    public Category? Category { get; private set; }
    
    private SubCategory() { }

    public static SubCategory Create(
        DateTimeOffset now,
        string name,
        string nameRu,
        string description,
        bool isActive,
        int categoryId)
    {
        var subCategory = new SubCategory()
        {
            
        };

        return subCategory;
    }

    public void ChangeName(
        DateTimeOffset now,
        string param)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        UpdatedAt = now;
    }
    
    public void ChangeNameRu(
        DateTimeOffset now,
        string param)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        UpdatedAt = now;
    }
    
    public void ChangeDescription(
        DateTimeOffset now,
        string param)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        UpdatedAt = now;
    }

    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Sub Category is deleted.");
    }
}