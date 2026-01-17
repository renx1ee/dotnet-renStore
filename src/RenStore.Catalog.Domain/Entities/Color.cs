using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a color physical entity with lifecycle and invariants.
/// </summary>
public class Color
{
    private readonly List<ProductVariantEntity> _variants = new();
    
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; } 
    public string NameRu { get; private set; } 
    public string NormalizedNameRu { get; private set; } 
    
    public ColorCode ColorCode { get; private set; }
    
    public bool IsDeleted { get; private set; } = false;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    public IReadOnlyCollection<ProductVariantEntity> ProductVariants => _variants.AsReadOnly();
    
    private Color() { }

    public static Color Create(
        DateTimeOffset now,
        string name,
        string nameRu,
        string colorCode)
    {
        string trimmedName   = name.Trim();
        string trimmedNameRu = nameRu.Trim();
        
        if(trimmedName.Length is < 1 or > 100)
            throw new DomainException("Color name must be 1-100 characters.");
        
        if(trimmedNameRu.Length is < 1 or > 100)
            throw new DomainException("Color name ru must be 1-100 characters.");
        
        if (string.IsNullOrWhiteSpace(trimmedName))
            throw new DomainException("Name cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(trimmedNameRu))
            throw new DomainException("Name Ru cannot be null or empty.");
        
        if (string.IsNullOrWhiteSpace(colorCode))
            throw new DomainException("Color Code cannot be null or empty.");
        
        var color = new Color()
        {
            Name = name.Trim(),
            NormalizedName = name.Trim().ToUpperInvariant(),
            NameRu = nameRu.Trim(),
            NormalizedNameRu = nameRu.Trim().ToUpperInvariant(),
            ColorCode = ColorCode.Create(colorCode),
            IsDeleted = false,
            CreatedAt = now
        };

        return color;
    }

    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted("Cannot change a name with deleted entity.");
        
        string trimmedName = name.Trim();

        if (trimmedName == Name) return;
        
        if(trimmedName.Length is < 1 or > 100)
            throw new DomainException("Color name must be 1-100 characters.");
        
        Name = trimmedName;
        NormalizedName = trimmedName.ToUpperInvariant();
        UpdatedAt = now;
    }
    
    public void ChangeNameRu(
        DateTimeOffset now,
        string nameRu)
    {
        EnsureNotDeleted("Cannot change a name ru with deleted entity.");
        
        string trimmedNameRu = nameRu.Trim();
        
        if (trimmedNameRu == NameRu) return;
        
        if(trimmedNameRu.Length is < 1 or > 100)
            throw new DomainException("Color name ru must be 1-100 characters.");
        
        NameRu = nameRu.Trim();
        NormalizedNameRu = nameRu.Trim().ToUpperInvariant();
        UpdatedAt = now;
    }

    public void ChangeColorCode(
        DateTimeOffset now,
        string colorCode)
    {
        EnsureNotDeleted("Cannot change a color code with deleted entity.");
        
        if (string.IsNullOrWhiteSpace(colorCode))
            throw new DomainException("Color Code cannot be null or empty.");
        
        ColorCode = ColorCode.Create(colorCode);
        UpdatedAt = now;
    }
    
    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted color.");

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }

    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Color already deleted.");
    }
}