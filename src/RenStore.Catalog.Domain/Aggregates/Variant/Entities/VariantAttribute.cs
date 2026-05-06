using RenStore.Catalog.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Entities;

public sealed class VariantAttribute
{
    /// <summary>
    /// Unique identifier of the product attribute.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Attribute name.
    /// Stored in uppercase for case-insensitive operations.
    /// </summary>
    public AttributeKey Key { get; private set; } 
    
    /// <summary>
    /// Attribute specification or value.
    /// </summary>
    public AttributeValue Value { get; private set; }
    
    /// <summary>
    /// Indicates whether this attribute has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; private set; }
    
    /// <summary>
    /// Date when the product was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    
    /// <summary>
    /// Date when the product was updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }
    
    /// <summary>
    /// Date when the product was deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; private set; }
    
    /// <summary>
    /// Identifier of the product variant this attribute describes.
    /// </summary>
    public Guid VariantId { get; private set; }
    
    private VariantAttribute() { }
    
    internal static VariantAttribute Create(
        DateTimeOffset now,
        Guid attributeId,
        Guid variantId,
        string key,
        string value)
    {
        return new VariantAttribute()
        {
            Id = attributeId,
            VariantId = variantId,
            CreatedAt = now,
            Key = AttributeKey.Create(key),
            Value = AttributeValue.Create(value),
            IsDeleted = false
        };
    }
    
    internal void ChangeKey(
        DateTimeOffset now,
        string key)
    {
        Key = AttributeKey.Create(key);
        UpdatedAt = now;
    }
    
    internal void ChangeValue(
        DateTimeOffset now,
        string value)
    {
        Value = AttributeValue.Create(value);
        UpdatedAt = now;
    }
    
    internal void Delete(
        DateTimeOffset now)
    {
        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    internal void Restore(
        DateTimeOffset now)
    {
        IsDeleted = false;
        DeletedAt = null;
        UpdatedAt = now;
    }
} 