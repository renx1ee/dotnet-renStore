using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

internal static class ProductAttributeRules
{
    private const int MaxKeyLength   = 100;
    private const int MinKeyLength   = 1;
    
    private const int MaxValueLength = 500;
    private const int MinValueLength = 1;
    
    internal static string KeyNormalizeAndValidate(string key)
    {
        if(string.IsNullOrWhiteSpace(key))
            throw new DomainException($"Attribute key cannot be null or whitespace."); 
        
        string trimmedKey = key.Trim();
        
        if (trimmedKey.Length is > MaxKeyLength or < MinKeyLength)
            throw new DomainException($"Attribute key must be between {MaxKeyLength} and {MinKeyLength}.");

        return trimmedKey;
    }
    
    internal static string ValueNormalizeAndValidate(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException($"Attribute value cannot be null or whitespace."); 
        
        var trimmedValue = value.Trim();
        
        if (trimmedValue.Length is > MaxValueLength or < MinValueLength)
            throw new DomainException($"Attribute value must be between {MaxValueLength} and {MinValueLength}.");

        return trimmedValue;
    }
    
    internal static void ProductVariantIdNormalizeAndValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException($"ProductVariantId cannot be guid empty.");
    }
}