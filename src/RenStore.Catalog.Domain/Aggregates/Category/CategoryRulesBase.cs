using RenStore.SharedKernal.Domain.Entities;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Category;

public abstract class CategoryRulesBase :
    EntityWithSoftDeleteBase
{
    private const int MaxCategoryNameLength = 100;
    private const int MinCategoryNameLength = 2;
    
    private const int MaxDescriptionLength  = 500;
    private const int MinDescriptionLength  = 1;
    
    private protected static string NormalizeAndValidateName(string name)
    {
        var trimmedName = name.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedName))
            throw new DomainException("Category name cannot be null or whitespace.");
        
        if(trimmedName.Length is < MinCategoryNameLength or > MaxCategoryNameLength)
            throw new DomainException($"Category nameRu length must be between {MaxCategoryNameLength} and {MinCategoryNameLength}.");

        return trimmedName;
    }
    
    private protected static string NormalizeAndValidateNameRu(string nameRu)
    {
        string trimmedNameRu = nameRu.Trim();
        
        if(string.IsNullOrWhiteSpace(trimmedNameRu))
            throw new DomainException("Category name ru cannot be null or whitespace.");
        
        if(trimmedNameRu.Length is < MinCategoryNameLength or > MaxCategoryNameLength)
            throw new DomainException($"Category nameRu ru length must be between {MaxCategoryNameLength} and {MinCategoryNameLength}.");

        return trimmedNameRu;
    }
    
    private protected static string? NormalizeAndValidateDescription(string? description)
    {
        if(description == null)
            throw new DomainException("Category description cannot be null.");
        
        var trimmedDescription = description.Trim();
        
        if(trimmedDescription.Length is > MaxDescriptionLength or < MinDescriptionLength)
            throw new DomainException($"Category description length must be between {MaxDescriptionLength} and {MinDescriptionLength}.");

        return trimmedDescription;
    }
}