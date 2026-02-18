using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.ValueObjects;

public record AttributeValue
{
    private const int MaxValueLength = 500;
    private const int MinValueLength = 1;
    
    public string Value { get; }

    private AttributeValue(string value)
    {
        Value = ValueNormalizeAndValidate(value);
    }

    public static AttributeValue Create(string value) => 
        new AttributeValue(value);
    
    public static string ValueNormalizeAndValidate(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException(
                "Attribute value cannot be null or whitespace."); 
        
        var trimmedValue = value.Trim();
        
        if (trimmedValue.Length is > MaxValueLength or < MinValueLength)
            throw new DomainException(
                $"Attribute value must be between {MaxValueLength} and {MinValueLength} characters.");

        return trimmedValue;
    }
    
    public static implicit operator string(AttributeValue value) => value.Value;

    public override string ToString() => Value;
}