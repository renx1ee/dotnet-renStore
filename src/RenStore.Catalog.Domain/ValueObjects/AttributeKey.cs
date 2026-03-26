using RenStore.Catalog.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.ValueObjects;

public record AttributeKey
{
    public string Key { get; }

    private AttributeKey(string key)
    {
        Key = KeyNormalizeAndValidate(key);
    }

    public static AttributeKey Create(string key) => 
        new AttributeKey(key);
    
    public static string KeyNormalizeAndValidate(string key)
    {
        if(string.IsNullOrWhiteSpace(key))
            throw new DomainException(
                "Attribute key cannot be null or whitespace."); 
        
        string trimmedKey = key.Trim();
        
        if (trimmedKey.Length is > CatalogConstants.Attribute.MaxKeyLength 
                              or < CatalogConstants.Attribute.MinKeyLength)
        {
            throw new DomainException(
                $"Attribute key must be between " +
                $"{CatalogConstants.Attribute.MaxKeyLength} and " +
                $"{CatalogConstants.Attribute.MinKeyLength} characters.");
        }

        return trimmedKey;
    }

    public static implicit operator string(AttributeKey key) => key.Key;

    public override string ToString() => Key;
}