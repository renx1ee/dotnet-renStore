using RenStore.Catalog.Domain.Constants;
using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

public class Color
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    public string NormalizedName { get; private set; } 
    public string NameRu { get; private set; } 
    public string NormalizedNameRu { get; private set; } 
    public bool IsDeleted { get; private set; }
    public ColorCode Code { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    private Color() { }

    public static Color Create(
        DateTimeOffset now,
        string name,
        string nameRu,
        string colorCode)
    {
        string trimmedName = NameValidation(name);
        string trimmedNameRu = NameRuValidation(nameRu);
        
        ColorCodeValidate(colorCode);
        
        var color = new Color()
        {
            Name = trimmedName,
            NormalizedName = trimmedName.ToUpperInvariant(),
            NameRu = trimmedNameRu,
            NormalizedNameRu = trimmedNameRu.ToUpperInvariant(),
            Code = ColorCode.Create(colorCode),
            IsDeleted = false,
            CreatedAt = now
        };

        return color;
    }

    public void ChangeName(
        DateTimeOffset now,
        string name)
    {
        EnsureNotDeleted("Cannot change a name with deleted color.");
        
        string trimmedName = NameValidation(name);

        if (trimmedName == Name) return;
        
        Name = trimmedName;
        NormalizedName = trimmedName.ToUpperInvariant();
        UpdatedAt = now;
    }
    
    public void ChangeNameRu(
        DateTimeOffset now,
        string nameRu)
    {
        EnsureNotDeleted("Cannot change a name ru with deleted entity.");
        
        string trimmedNameRu = NameRuValidation(nameRu);
        
        if (trimmedNameRu == NameRu) return;
        
        NameRu = nameRu.Trim();
        NormalizedNameRu = nameRu.Trim().ToUpperInvariant();
        UpdatedAt = now;
    }

    public void ChangeColorCode(
        DateTimeOffset now,
        string colorCode)
    {
        EnsureNotDeleted("Cannot change a color code with deleted entity.");
        
        ColorCodeValidate(colorCode);
        
        Code = ColorCode.Create(colorCode);
        UpdatedAt = now;
    }
    
    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted("Cannot delete already deleted color.");

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    // TODO: restore
    

    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Color already deleted.");
    }

    private static string NameValidation(string name)
    {
        string trimmedName = name.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmedName))
            throw new DomainException("Key cannot be null or empty.");
        
        if(trimmedName.Length is < CatalogConstants.Color.MinColorNameLength 
                              or > CatalogConstants.Color.MaxColorNameLength)
            throw new DomainException(
                "Color name must have between " +
                $"{CatalogConstants.Color.MinColorNameLength} and " +
                $"{CatalogConstants.Color.MaxColorNameLength} characters.");

        return trimmedName;
    }
    
    private static string NameRuValidation(string nameRu)
    {
        string trimmedNameRu = nameRu.Trim();
        
        if (string.IsNullOrWhiteSpace(trimmedNameRu))
            throw new DomainException("Key Ru cannot be null or empty.");
        
        if(trimmedNameRu.Length is < CatalogConstants.Color.MinColorNameLength 
           or > CatalogConstants.Color.MaxColorNameLength)
            throw new DomainException(
                "Color name ru must have between " +
                $"{CatalogConstants.Color.MinColorNameLength} and " +
                $"{CatalogConstants.Color.MaxColorNameLength} characters.");
        
        return trimmedNameRu;
    }

    private static void ColorCodeValidate(string colorCode)
    {
        if (string.IsNullOrWhiteSpace(colorCode))
            throw new DomainException("Color Code cannot be null or empty.");
    }
}