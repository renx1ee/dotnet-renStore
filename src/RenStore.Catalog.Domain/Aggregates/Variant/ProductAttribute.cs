using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a product attribute physical entity with lifecycle and invariants.
/// </summary>
public class ProductAttribute
{
    public Guid Id { get; private set; }
    public string Key { get; private set; } 
    public string Value { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    private ProductAttribute() { }

    internal static ProductAttribute Create(
        string key,
        string value,
        Guid productVariantId,
        DateTimeOffset now)
    {
        return new ProductAttribute()
        {
            Key = key.ToUpperInvariant(),
            Value = value,
            ProductVariantId = productVariantId,
            CreatedAt = now,
            IsDeleted = false
        };
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
        
        var trimmedKey = ProductAttributeRules.KeyNormalizeAndValidate(key);
        
        if(Key == key) return;

        Key = trimmedKey.ToUpperInvariant();
        UpdatedAt = now;
    }

    public void ChangeValue(
        DateTimeOffset now,
        string value)
    {
        EnsureNotDeleted();
        
        var trimmedValue = ProductAttributeRules.ValueNormalizeAndValidate(value);
        
        if(Value == value) return;

        Value = trimmedValue;
        UpdatedAt = now;
    }

    internal void Delete(DateTimeOffset now)
    {
        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    internal void Restore(DateTimeOffset now)
    {
        IsDeleted = true;
        UpdatedAt = now;
        DeletedAt = null;
    }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}