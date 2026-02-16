using RenStore.Catalog.Domain.Aggregates.Attribute.Events;
using RenStore.Catalog.Domain.Aggregates.Attribute.Rules;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Attribute;

/// <summary>
/// Represents a descriptive attribute attached to a product variant.
/// Attributes provide detailed specifications that help customers evaluate products.
/// </summary>
public class VariantAttribute
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    /// <summary>
    /// Unique identifier of the product attribute.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    /// Attribute name.
    /// Stored in uppercase for case-insensitive operations.
    /// </summary>
    public string Key { get; private set; } 
    
    /// <summary>
    /// Attribute specification or value.
    /// </summary>
    public string Value { get; private set; }
    
    /// <summary>
    /// Identifier of the product variant this attribute describes.
    /// </summary>
    public Guid ProductVariantId { get; private set; }
    
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
    
    private VariantAttribute() { }
    
    public static VariantAttribute Create(
        DateTimeOffset now,
        Guid variantId,
        string key,
        string value)
    {
        ProductAttributeRules.ProductVariantIdNormalizeAndValidate(variantId);
        
        var trimmedKey   = ProductAttributeRules.KeyNormalizeAndValidate(key);
        var trimmedValue = ProductAttributeRules.ValueNormalizeAndValidate(value);

        var attributeId = Guid.NewGuid();
        var attribute = new VariantAttribute();
        
        attribute.Raise(new AttributeCreated(
            VariantId: variantId,
            AttributeId: attributeId,
            OccurredAt: now,
            Key: trimmedKey,
            Value: trimmedValue));

        return attribute;
    }
    
    public void ChangeKey(
        DateTimeOffset now,
        Guid variantId,
        Guid attributeId,
        string key)
    {
        EnsureNotDeleted();
        
        var trimmedKey = ProductAttributeRules
            .KeyNormalizeAndValidate(key);
        
        if(Key == trimmedKey) return;
        
        Raise(new AttributeKeyUpdated(
            OccurredAt: now,
            VariantId: variantId,
            AttributeId: attributeId,
            Key: trimmedKey));
    }
    
    public void ChangeValue(
        DateTimeOffset now,
        Guid variantId,
        Guid attributeId,
        string value)
    {
        EnsureNotDeleted();
        
        var trimmedValue = ProductAttributeRules
            .ValueNormalizeAndValidate(value);
        
        if(Value == trimmedValue) return;
        
        Raise(new AttributeValueUpdated(
            OccurredAt: now,
            VariantId: variantId,
            AttributeId: attributeId,
            Value: trimmedValue));
    }
    
    public void Delete(
        DateTimeOffset now,
        Guid variantId,
        Guid attributeId)
    {
        if (IsDeleted)
            throw new DomainException("Attribute already was deleted");
        
        Raise(new AttributeRemoved(
            OccurredAt: now,
            VariantId: variantId,
            AttributeId: attributeId));
    }
    
    public void Restore(
        DateTimeOffset now,
        Guid variantId,
        Guid attributeId)
    {
        if (!IsDeleted)
            throw new DomainException("Attribute wasn't deleted.");
        
        Raise(new AttributeRestored(
            OccurredAt: now,
            VariantId: variantId,
            AttributeId: attributeId));
    }
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case AttributeCreated e:
                Id = e.AttributeId;
                Key = e.Key;
                Value = e.Value;
                ProductVariantId = e.VariantId;
                CreatedAt = e.OccurredAt;
                IsDeleted = false;
                break;
            
            case AttributeKeyUpdated e:
                Key = e.Key.ToUpperInvariant();
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeValueUpdated e:
                Value = e.Value;
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeRemoved e:
                IsDeleted = true;
                DeletedAt = e.OccurredAt;
                UpdatedAt = e.OccurredAt;
                break;
            
            case AttributeRestored e:
                IsDeleted = false;
                DeletedAt = null;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
    
    /// <summary>
    /// Ensures the variant attribute is not deleted before performing operations.
    /// </summary>
    /// <exception cref="DomainException">Thrown when attribute is deleted</exception>
    private void EnsureNotDeleted()
    {
        if(IsDeleted)
            throw new DomainException("Attribute already was deleted.");
    }
}

/*/// <summary>
/// Reconstructs a product attribute from persistence.
/// </summary>
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
}*/