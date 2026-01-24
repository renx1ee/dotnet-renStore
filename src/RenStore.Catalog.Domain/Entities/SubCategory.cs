using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a sub category physical entity with lifecycle and invariants.
/// </summary>
public class SubCategory
{
    private Category? _category { get; set; }
    
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; }
    public string NameRu { get; private set; }
    public string NormalizedNameRu { get; private set; }
    public string Description { get; private set; }
    
    
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    public int CategoryId { get; private set; }
    
    private const int MaxCategoryNameLength = 100;
    private const int MinCategoryNameLength = 2;
    
    private const int MaxDescriptionLength  = 500;
    private const int MinDescriptionLength  = 25;
    
    private SubCategory() { }

    public static SubCategory Create(
        int categoryId,
        DateTimeOffset now,
        string name,
        string nameRu,
        string? description = null)
    {
        CategoryIdValidation(categoryId);
        
        string trimmedName   = SubCategoryNameValidation(name);
        string trimmedNameRu = SubCategoryNameRuValidation(nameRu);
        
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
            string trimmedDescription = SubCategoryDescriptionValidation(description);
            
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
        
        string trimmedName = SubCategoryNameValidation(name);
        
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
        
        string trimmedNameRu = SubCategoryNameRuValidation(nameRu);
        
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
        
        string trimmedDescription = SubCategoryDescriptionValidation(description);
        
        if (trimmedDescription == Description) return;
        
        UpdatedAt = now;
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted sub category.");
        
        IsDeleted = true;
        
        DeletedAt = now;
        UpdatedAt = now;
    }

    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Sub Category is deleted.");
    }
    
    private static void CategoryIdValidation(int categoryId)
    {
        if(categoryId <= 0)
            throw new DomainException("Category Id must be greater then 1.");
    } 
    private static string SubCategoryNameValidation(string name)
    {
        var trimmedName = name.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedName))
            throw new DomainException("Category name cannot be null or whitespace.");
        
        if(trimmedName.Length is < MinCategoryNameLength or > MaxCategoryNameLength)
            throw new DomainException($"Category nameRu length must be between {MaxCategoryNameLength} and {MinCategoryNameLength}.");

        return trimmedName;
    }
    
    private static string SubCategoryNameRuValidation(string nameRu)
    {
        string trimmedNameRu = nameRu.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedNameRu))
            throw new DomainException("Category name ru cannot be null or whitespace.");
        
        if(trimmedNameRu.Length is < MinCategoryNameLength or > MaxCategoryNameLength)
            throw new DomainException($"Category nameRu ru length must be between {MaxCategoryNameLength} and {MinCategoryNameLength}.");

        return trimmedNameRu;
    }
    
    private static string SubCategoryDescriptionValidation(string description)
    {
        var trimmedDescription = description.Trim();
        
        if(description.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Category description length must be between {MaxDescriptionLength} and {MinDescriptionLength}.");

        return trimmedDescription;
    }
}