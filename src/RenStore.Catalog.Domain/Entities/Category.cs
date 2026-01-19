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
    
    private Category() { }

    public static Category Create(
        DateTimeOffset now,
        string name,
        string nameRu,
        string? description = null)
    {
        string trimmedName   = name.Trim();
        string trimmedNameRu = nameRu.Trim();
        
        if(trimmedName.Length is < 1 or > 100)
            throw new DomainException("Category name must be 1-100 characters.");
        
        if(trimmedNameRu.Length is < 1 or > 100)
            throw new DomainException("Category name ru must be 1-100 characters.");
        
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
            string trimmedDescription = description.Trim();
            
            if (trimmedDescription.Length is > 10 and < 500)
                category.Description = trimmedDescription;
        }
        
        return category;
    }

    public void AddSubCategory(
        SubCategory subCategory)
    {
        EnsureNotDeleted();
        
        _subCategories.Add(subCategory);
    }
    
    public void AddRangeOfSubCategory(
        IReadOnlyList<SubCategory> subCategories)
    {
        EnsureNotDeleted();

        var list = subCategories as List<SubCategory> ?? subCategories.ToList();

        if (!list.Any()) return;
        
        _subCategories.AddRange(list);
    }

    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        string trimmedName = name.Trim();
        
        if (trimmedName == Name) return;
        
        if(trimmedName.Length is < 1 or > 100)
            throw new DomainException("Category name must be 1-100 characters.");

        Name = trimmedName;
        NormalizedName = trimmedName.ToUpperInvariant();
        
        UpdatedAt = now;
    }
    
    public void ChangeNameRu(
        DateTimeOffset now,
        string nameRu)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        string trimmedNameRu = nameRu.Trim();
        
        if (trimmedNameRu == NameRu) return;
        
        if(trimmedNameRu.Length is < 1 or > 100)
            throw new DomainException("Category name ru must be 1-100 characters.");

        NameRu = trimmedNameRu;
        NormalizedNameRu = trimmedNameRu.ToUpperInvariant();
        
        UpdatedAt = now;
    }
    
    public void ChangeDescription(
        DateTimeOffset now,
        string description)
    {
        EnsureNotDeleted("Cannot change deleted category.");
        
        string trimmedDescription = description.Trim();
        
        if (trimmedDescription == Description) return;
            
        if (trimmedDescription.Length is > 10 and < 500)
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

    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Category is deleted.");
    }
}