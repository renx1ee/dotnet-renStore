using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.ValueObjects;

public record AttributeKey
{
    private const int MaxKeyLength = 100;
    private const int MinKeyLength = 1;
    
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
        
        if (trimmedKey.Length is > MaxKeyLength or < MinKeyLength)
            throw new DomainException(
                $"Attribute key must be between {MaxKeyLength} and {MinKeyLength} characters.");

        return trimmedKey;
    }

    public static implicit operator string(AttributeKey key) => key.Key;

    public override string ToString() => Key;
}