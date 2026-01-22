using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product attribute physical entity with lifecycle and invariants.
/// </summary>
public class ProductAttributeEntity
{
    private ProductVariant? _productVariant { get; set; }
    
    public Guid Id { get; private set; }
    public string Key { get; private set; } 
    public string Value { get; private set; }
    public Guid ProductVariantId { get; private set; }
    
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    private const int MaxKeyLength = 100;
    private const int MinKeyLength = 1;
    
    private const int MaxValueLength = 500;
    private const int MinValueLength = 1;

    private ProductAttributeEntity() { }

    public static ProductAttributeEntity Create(
        string key,
        string value,
        Guid productVariantId,
        DateTimeOffset now)
    {
        if(string.IsNullOrWhiteSpace(key))
            throw new DomainException($"Attribute key cannot be null or whitespace."); 
        
        var trimmedKey = key.Trim();
        
        if (trimmedKey.Length is > MaxKeyLength or < MinKeyLength)
            throw new DomainException($"Attribute key must be between {MaxKeyLength} and {MinKeyLength}");
        
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException($"Attribute value cannot be null or whitespace."); 
        
        var trimmedValue = value.Trim();
        
        if (trimmedValue.Length is > MaxValueLength or < MinValueLength)
            throw new DomainException($"Attribute value must be between {MaxValueLength} and {MinValueLength}");
        
        if(productVariantId == Guid.Empty)
            throw new DomainException($"ProductVariantId cannot be guid empty");
        
        var attribute = new ProductAttributeEntity()
        {
            Key = trimmedKey.ToUpperInvariant(),
            Value = trimmedValue,
            ProductVariantId = productVariantId,
            CreatedAt = now,
            IsDeleted = false
        };

        return attribute;
    }
    
    public void ChangeKey(
        DateTimeOffset now,
        string key)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(key))
            throw new DomainException($"Attribute key cannot be null or whitespace."); 
        
        var trimmedKey = key.Trim();
        
        if (trimmedKey.Length is > MaxKeyLength or < MinKeyLength)
            throw new DomainException($"Attribute key must be between {MaxKeyLength} and {MinKeyLength}");

        Key = trimmedKey.ToUpperInvariant();
        UpdatedAt = now;
    }

    public void ChangeValue(
        DateTimeOffset now,
        string value)
    {
        EnsureNotDeleted();
        
        if(string.IsNullOrWhiteSpace(value))
            throw new DomainException($"Attribute value cannot be null or whitespace."); 
        
        var trimmedValue = value.Trim();
        
        if (trimmedValue.Length is > MaxValueLength or < MinValueLength)
            throw new DomainException($"Attribute value must be between {MaxValueLength} and {MinValueLength}");

        Value = trimmedValue;
        UpdatedAt = now;
    }
    
    

    public void Delete(DateTimeOffset now)
    {
        EnsureNotDeleted();

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    public void Restore(DateTimeOffset now)
    {
        if(!IsDeleted)
            throw new DomainException($"Product attribute is not deleted!");

        IsDeleted = false;
        UpdatedAt = now;
        DeletedAt = null;
    }
    
    private void EnsureNotDeleted()
    {
        if(IsDeleted)
            throw new DomainException($"Product attribute already was deleted!");
    }
}