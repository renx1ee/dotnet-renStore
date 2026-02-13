using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.VariantAttributes.Rules;

/// <summary>
/// Business rules and validation logic for product attributes.
/// Ensures data integrity and enforces domain constraints on attribute creation and modification.
/// </summary>
internal static class ProductAttributeRules
{
    private const int MaxKeyLength   = 100;
    private const int MinKeyLength   = 1;
    
    private const int MaxValueLength = 500;
    private const int MinValueLength = 1;
    
    /// <summary>
    /// Normalizes and validates an attribute key (category name).
    /// </summary>
    /// <param name="key">Attribute key to validate</param>
    /// <returns>Trimmed and validated key</returns>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Key is null, empty, or whitespace
    /// - Key length is less than <see cref="MinKeyLength"/> or greater than <see cref="MaxKeyLength"/>
    /// </exception>
    /// <remarks>
    /// Keys typically represent attribute categories like "Material", "Screen Size", or "Weight".
    /// Validation ensures consistent data quality for filtering and display purposes.
    /// </remarks>
    internal static string KeyNormalizeAndValidate(string key)
    {
        if(string.IsNullOrWhiteSpace(key))
            throw new DomainException($"Attribute key cannot be null or whitespace."); 
        
        string trimmedKey = key.Trim();
        
        if (trimmedKey.Length is > MaxKeyLength or < MinKeyLength)
            throw new DomainException($"Attribute key must be between {MaxKeyLength} and {MinKeyLength}.");

        return trimmedKey;
    }
    
    /// <summary>
    /// Normalizes and validates an attribute value (specification).
    /// </summary>
    /// <param name="value">Attribute value to validate</param>
    /// <returns>Trimmed and validated value</returns>
    /// <exception cref="DomainException">
    /// Thrown when:
    /// - Value is null, empty, or whitespace
    /// - Value length is less than <see cref="MinValueLength"/> or greater than <see cref="MaxValueLength"/>
    /// </exception>
    /// <remarks>
    /// Values specify the actual attribute data like "Leather", "6.1 inches", or "1.5kg".
    /// Allows for descriptive values while maintaining performance constraints.
    /// </remarks>
    internal static string ValueNormalizeAndValidate(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException($"Attribute value cannot be null or whitespace."); 
        
        var trimmedValue = value.Trim();
        
        if (trimmedValue.Length is > MaxValueLength or < MinValueLength)
            throw new DomainException($"Attribute value must be between {MaxValueLength} and {MinValueLength}.");

        return trimmedValue;
    }
    
    /// <summary>
    /// Validates a product variant identifier.
    /// </summary>
    /// <param name="productVariantId">Variant identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is <see cref="Guid.Empty"/>
    /// </exception>
    /// <remarks>
    /// Ensures attributes are always linked to a valid parent variant.
    /// Empty GUIDs indicate uninitialized or invalid relationships.
    /// </remarks>
    internal static void ProductVariantIdNormalizeAndValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException($"ProductVariantId cannot be guid empty.");
    }
}