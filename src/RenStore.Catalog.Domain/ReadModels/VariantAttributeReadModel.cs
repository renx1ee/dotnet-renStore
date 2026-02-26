using RenStore.Catalog.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.ReadModels;

public class VariantAttributeReadModel
{
    /// <summary>
    /// Unique identifier of the product attribute.
    /// </summary>
    public Guid Id { get; init; }
    // TODO:
    /// <summary>
    /// Attribute name.
    /// Stored in uppercase for case-insensitive operations.
    /// </summary>
    public AttributeKey Key { get; init; } 
    
    /// <summary>
    /// Attribute specification or value.
    /// </summary>
    public AttributeValue Value { get; init; }
    
    /// <summary>
    /// Indicates whether this attribute has been soft-deleted.
    /// </summary>
    public bool IsDeleted { get; init; }
    
    /// <summary>
    /// Date when the product was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; init; }
    
    /// <summary>
    /// Date when the product was updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; init; }
    
    /// <summary>
    /// Date when the product was deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; init; }
    
    /// <summary>
    /// Identifier of the product variant this attribute describes.
    /// </summary>
    public Guid VariantId { get; init; }
}