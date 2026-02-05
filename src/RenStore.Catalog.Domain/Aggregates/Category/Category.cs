using RenStore.Catalog.Domain.Entities;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Category;

/// <summary>
/// Represents a category physical entity with lifecycle and invariants.
/// </summary>
public class Category :
    CategoryRulesBase
{
    private readonly List<SubCategory> _subCategories = new();
    
    public int Id { get; private set; }
    public string Name { get; private set; } 
    public string NormalizedName { get; private set; }
    public string NameRu { get; private set; } 
    public string NormalizedNameRu { get; private set; } 
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } // TODO:
    public DateTimeOffset CreatedAt { get; private set; } 
    
    public DateTimeOffset? UpdatedAt { get; protected set; }
    public DateTimeOffset? DeletedAt { get; protected set; }
    public IReadOnlyCollection<SubCategory> SubCategories => _subCategories.AsReadOnly();
    
    private Category() { }
    
    public static Category Create(
        DateTimeOffset now,
        string name,
        string nameRu,
        string? description = null)
    {
        string trimmedName   = NormalizeAndValidateName(name);
        string trimmedNameRu = NormalizeAndValidateNameRu(nameRu);
        
        var category = new Category()
        {
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

    public static Category Reconstitute(
        int id,
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
        var category = new Category()
        {
            Id = id,
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
        EnsureNotDeleted("Cannot change deleted category.");
        
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
        EnsureNotDeleted("Cannot change deleted category.");
        
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
        EnsureNotDeleted("Cannot change deleted category.");
        
        string? trimmedDescription = NormalizeAndValidateDescription(description);
        
        if (trimmedDescription == Description) return;

        Description = trimmedDescription;
        UpdatedAt = now;
    }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}