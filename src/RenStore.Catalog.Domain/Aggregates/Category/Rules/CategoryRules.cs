using RenStore.Catalog.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Category.Rules;

public static class CategoryRules 
{
    internal static string NormalizeAndValidateName(string name)
    {
        var trimmedName = name.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedName))
            throw new DomainException("Category name cannot be null or whitespace.");

        if (trimmedName.Length is < CatalogConstants.Category.MinCategoryNameLength
                               or > CatalogConstants.Category.MaxCategoryNameLength)
        {
            throw new DomainException(
                $"Category nameRu length must be between " +
                $"{CatalogConstants.Category.MaxCategoryNameLength} and " +
                $"{CatalogConstants.Category.MinCategoryNameLength}.");
        }
        
        return trimmedName;
    }
    
    internal static string NormalizeAndValidateNameRu(string nameRu)
    {
        string trimmedNameRu = nameRu.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedNameRu))
            throw new DomainException("Category name ru cannot be null or whitespace.");
        
        if(trimmedNameRu.Length is < CatalogConstants.Category.MinCategoryNameLength 
                                or > CatalogConstants.Category.MaxCategoryNameLength)
        {
            throw new DomainException(
                $"Category nameRu ru length must be between " +
                $"{CatalogConstants.Category.MaxCategoryNameLength} and " +
                $"{CatalogConstants.Category.MinCategoryNameLength}.");
        }

        return trimmedNameRu;
    }
    
    internal static string? NormalizeAndValidateDescription(string? description)
    {
        if (description == null) return null;
        
        var trimmedDescription = description.Trim();
        
        if(trimmedDescription.Length is > CatalogConstants.Category.MaxDescriptionLength 
                                     or < CatalogConstants.Category.MinDescriptionLength)
        {
            throw new DomainException(
                $"Category description length must be between " +
                $"{CatalogConstants.Category.MaxDescriptionLength} and " +
                $"{CatalogConstants.Category.MinDescriptionLength}.");
        }

        return trimmedDescription;
    }
    
    internal static void UpdatedByValidation(
        Guid updatedById,
        string updatedByRole)
    {
        if (updatedById == Guid.Empty)
            throw new DomainException("Updated by ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException("Updated by role cannot be empty string");
    }
}