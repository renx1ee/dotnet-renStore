using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a sub category physical entity with lifecycle and invariants.
/// </summary>
public class SubCategory : 
    RenStore.Catalog.Domain.Entities.CategoryRulesBase
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public string NameRu { get; private set; }
    public string NormalizedNameRu { get; private set; }
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } // TODO:
    public DateTimeOffset CreatedAt { get; private set; } 
    public int CategoryId { get; private set; }
    
    private SubCategory() { }

    public static SubCategory Create(
        int categoryId,
        DateTimeOffset now,
        string name,
        string nameRu,
        string? description = null)
    {
        CategoryIdValidation(categoryId);
        
        string trimmedName   = NormalizeAndValidateName(name);
        string trimmedNameRu = NormalizeAndValidateNameRu(nameRu);
        
        var category = new SubCategory()
        {
            CategoryId = categoryId,
            Name = trimmedName,
            NormalizedName = trimmedName.ToUpperInvariant(),
            NameRu = trimmedNameRu,
            NormalizedNameRu = trimmedNameRu.ToUpperInvariant(),
            IsDeleted = false,
            CreatedAt = now
        };

        if (!string.IsNullOrWhiteSpace(description))
        {
            string? trimmedDescription = NormalizeAndValidateDescription(description);
            
            category.Description = trimmedDescription;
        }
        
        return category;
    }
    
    public static SubCategory Reconstitute(
        int id,
        int categoryId,
        string name,
        string normalizedName,
        string nameRu,
        string normalizedNameRu,
        string description,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        DateTimeOffset? deletedAt)
    {
        var category = new SubCategory()
        {
            Id = id,
            CategoryId = categoryId,
            Name = name,
            NormalizedName = normalizedName,
            NameRu = nameRu,
            NormalizedNameRu = normalizedNameRu,
            Description = description,
            IsDeleted = isDeleted,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
        
        return category;
    }

    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        string trimmedName = NormalizeAndValidateName(name);
        
        if (trimmedName == Name) return;

        Name = trimmedName;
        NormalizedName = trimmedName.ToUpperInvariant();
        
        UpdatedAt = now;
    }
    
    public void ChangeNameRu(
        DateTimeOffset now,
        string nameRu)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        string trimmedNameRu = NormalizeAndValidateNameRu(nameRu);
        
        if (trimmedNameRu == NameRu) return;

        NameRu = trimmedNameRu;
        NormalizedNameRu = trimmedNameRu.ToUpperInvariant();
        
        UpdatedAt = now;
    }
    
    public void ChangeDescription(
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        string? trimmedDescription = NormalizeAndValidateDescription(description);
        
        if (trimmedDescription == Description) return;

        Description = trimmedDescription;
        UpdatedAt = now;
    }
    
    private static void CategoryIdValidation(int categoryId)
    {
        if(categoryId <= 0)
            throw new DomainException("Category Id must be greater than 1.");
    }
}