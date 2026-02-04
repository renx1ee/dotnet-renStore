using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product attribute physical entity with lifecycle and invariants.
/// </summary>
public class ProductAttribute
    : RenStore.Catalog.Domain.Entities.EntityWithSoftDeleteBase
{
    public Guid Id { get; private set; }
    public string Key { get; private set; } 
    public string Value { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private const int MaxKeyLength   = 100;
    private const int MinKeyLength   = 1;
    
    private const int MaxValueLength = 500;
    private const int MinValueLength = 1;

    private ProductAttribute() { }

    internal static ProductAttribute Create(
        string key,
        string value,
        Guid productVariantId,
        DateTimeOffset now)
    {
        ProductVariantValidation(productVariantId);
        
        var trimmedKey   = KeyValidation(key);
        var trimmedValue = ValueValidation(value);
        
        var attribute = new ProductAttribute()
        {
            Key = trimmedKey.ToUpperInvariant(),
            Value = trimmedValue,
            ProductVariantId = productVariantId,
            CreatedAt = now,
            IsDeleted = false
        };

        return attribute;
    }
    
    public static ProductAttribute Reconstitute(
        Guid id,
        string key,
        string value,
        Guid productVariantId,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        DateTimeOffset? deletedAt)
    {
        var attribute = new ProductAttribute()
        {
            Id = id,
            Key = key,
            Value = value,
            ProductVariantId = productVariantId,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt,
            IsDeleted = isDeleted
        };

        return attribute;
    }
    
    public void ChangeKey(
        DateTimeOffset now,
        string key)
    {
        EnsureNotDeleted();
        
        var trimmedKey = KeyValidation(key);
        
        if(Key == key) return;

        Key = trimmedKey.ToUpperInvariant();
        UpdatedAt = now;
    }

    public void ChangeValue(
        DateTimeOffset now,
        string value)
    {
        EnsureNotDeleted();
        
        var trimmedValue = ValueValidation(value);
        
        if(Value == value) return;

        Value = trimmedValue;
        UpdatedAt = now;
    }

    private static string KeyValidation(string key)
    {
        if(string.IsNullOrWhiteSpace(key))
            throw new DomainException($"Attribute key cannot be null or whitespace."); 
        
        string trimmedKey = key.Trim();
        
        if (trimmedKey.Length is > MaxKeyLength or < MinKeyLength)
            throw new DomainException($"Attribute key must be between {MaxKeyLength} and {MinKeyLength}.");

        return trimmedKey;
    }
    
    private static string ValueValidation(string value)
    {
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException($"Attribute value cannot be null or whitespace."); 
        
        var trimmedValue = value.Trim();
        
        if (trimmedValue.Length is > MaxValueLength or < MinValueLength)
            throw new DomainException($"Attribute value must be between {MaxValueLength} and {MinValueLength}.");

        return trimmedValue;
    }
    
    private static void ProductVariantValidation(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException($"ProductVariantId cannot be guid empty.");
    }
}