using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a category physical entity with lifecycle and invariants.
/// </summary>
public class Category
{
    private readonly List<SubCategory> _subCategories = new();
    
    public int Id { get; private set; }
    
    public string Name { get; private set; } 
    public string NormalizedName { get; private set; }
    
    public string NameRu { get; private set; } 
    public string NormalizedNameRu { get; private set; } 
    
    public string? Description { get; private set; }
    
    
    public bool IsDeleted { get; private set; }
    
    public DateTimeOffset CreatedAt { get; private set; } 
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    public IReadOnlyCollection<SubCategory> SubCategories => _subCategories.AsReadOnly();

    private const int MaxCategoryNameLength = 100;
    private const int MinCategoryNameLength = 2;
    
    private const int MaxDescriptionLength  = 500;
    private const int MinDescriptionLength  = 25;
    
    private Category() { }

    #region Category
    public static Category Create(
        DateTimeOffset now,
        string name,
        string nameRu,
        string? description = null)
    {
        string trimmedName   = CategoryNameValidation(name);
        string trimmedNameRu = CategoryNameRuValidation(nameRu);
        
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
            string trimmedDescription = CategoryDescriptionValidation(description);

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
        
        string trimmedName = CategoryNameValidation(name);
        
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
        
        string trimmedNameRu = CategoryNameRuValidation(nameRu);
        
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
        
        string trimmedDescription = CategoryDescriptionValidation(description);
        
        if (trimmedDescription == Description) return;

        Description = trimmedDescription;
        UpdatedAt = now;
    }

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted category.");
        
        IsDeleted = true;
        
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    public void Restore(DateTimeOffset now)
    {
        if (!IsDeleted)
            throw new DomainException("Category is not was deleted.");
        
        IsDeleted = false;
        
        UpdatedAt = now;
        DeletedAt = null;
    }
    #endregion
    
    #region SubCategory
    public void AddSubCategory(
        SubCategory subCategory)
    {
        EnsureNotDeleted();

        if (subCategory == null)
            throw new DomainException("Sub category cannot be null");
        
        _subCategories.Add(subCategory);
    }
    
    public void AddRangeOfSubCategory(
        IReadOnlyList<SubCategory> subCategories)
    {
        EnsureNotDeleted();
        
        if(subCategories == null)
            throw new DomainException("Sub categories cannot be null");

        var list = subCategories as List<SubCategory> ?? subCategories.ToList();

        if (!list.Any()) return;
        
        _subCategories.AddRange(list);
    }

    public void RemoveSubCategory(
        DateTimeOffset now,
        SubCategory subCategory)
    {
        if(subCategory == null)
            throw new DomainException("Sub category cannot be null");
        
        subCategory.Delete(now);
    }
    
    public void RemoveSubCategories(
        DateTimeOffset now,
        IReadOnlyList<SubCategory> subCategories)
    {
        if(subCategories == null || !subCategories.Any())
            throw new DomainException("Sub categories cannot be null");

        foreach (var subCategory in subCategories)
            RemoveSubCategory(now, subCategory);
    }

    public void ChangeSubCategoryName(
        DateTimeOffset now,
        SubCategory subCategory,
        string name)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        if(string.IsNullOrWhiteSpace(name))
            throw new DomainException("Sub Category nameRu cannot be null or whitespace.");
        
        subCategory.ChangeName(
            name: name, 
            now: now);
    }
    
    public void ChangeSubCategoryNameRu(
        DateTimeOffset now,
        SubCategory subCategory,
        string nameRu)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        if(string.IsNullOrWhiteSpace(nameRu))
            throw new DomainException("Sub Category nameRu ru cannot be null or whitespace.");
        
        subCategory.ChangeNameRu(
            nameRu: nameRu,  
            now: now);
    }
    
    public void ChangeSubCategoryDescription(
        DateTimeOffset now,
        SubCategory subCategory,
        string description)
    {
        EnsureNotDeleted("Cannot change deleted sub category.");
        
        if(string.IsNullOrWhiteSpace(description))
            throw new DomainException("Sub Category description cannot be null or whitespace.");
        
        subCategory.ChangeDescription(
            description: description, 
            now: now);
    }

    #endregion

    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Category is deleted.");
    }

    private static string CategoryNameValidation(string name)
    {
        var trimmedName = name.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedName))
            throw new DomainException("Category name cannot be null or whitespace.");
        
        if(trimmedName.Length is < MinCategoryNameLength or > MaxCategoryNameLength)
            throw new DomainException($"Category nameRu length must be between {MaxCategoryNameLength} and {MinCategoryNameLength}.");

        return trimmedName;
    }
    
    private static string CategoryNameRuValidation(string nameRu)
    {
        string trimmedNameRu = nameRu.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedNameRu))
            throw new DomainException("Category name ru cannot be null or whitespace.");
        
        if(trimmedNameRu.Length is < MinCategoryNameLength or > MaxCategoryNameLength)
            throw new DomainException($"Category nameRu ru length must be between {MaxCategoryNameLength} and {MinCategoryNameLength}.");

        return trimmedNameRu;
    }
    
    private static string CategoryDescriptionValidation(string description)
    {
        var trimmedDescription = description.Trim();
        
        if(description.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Category description length must be between {MaxDescriptionLength} and {MinDescriptionLength}.");

        return trimmedDescription;
    }
}