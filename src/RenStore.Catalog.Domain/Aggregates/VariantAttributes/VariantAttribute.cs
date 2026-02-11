namespace RenStore.Catalog.Domain.Aggregates.VariantAttributes;

/// <summary>
/// Represents a descriptive attribute attached to a product variant.
/// Attributes provide detailed specifications that help customers evaluate products.
/// </summary>
public class VariantAttribute
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
    
    /// <summary>
    /// Creates a new product attribute with validated business rules.
    /// </summary>
    /// <param name="key">Attribute name.</param>
    /// <param name="value">Attribute specification.</param>
    /// <param name="productVariantId">Parent variant identifier.</param>
    /// <param name="now">Timestamp when the operation occurs. Used for event history.</param>
    /// <returns>A new product attribute instance.</returns>
    /// <remarks>
    /// Keys are normalized to uppercase for consistent comparison and indexing.
    /// Both key and value are validated against domain rules before creation.
    /// </remarks>
    internal static VariantAttribute Create(
        string key,
        string value,
        Guid productVariantId,
        DateTimeOffset now)
    {
        return new VariantAttribute()
        {
            Key = key.ToUpperInvariant(),
            Value = value,
            ProductVariantId = productVariantId,
            CreatedAt = now,
            IsDeleted = false
        };
    }
    
    /// <summary>
    /// Updates the attribute key while preserving the value.
    /// </summary>
    /// <param name="now">Update timestamp</param>
    /// <param name="key">New attribute category</param>
    internal void ChangeKey(
        DateTimeOffset now,
        string key)
    {
        Key = key.ToUpperInvariant();
        UpdatedAt = now;
    }

    /// <summary>
    /// Updates the attribute value while preserving the key.
    /// </summary>
    /// <param name="now">Update timestamp</param>
    /// <param name="value">New attribute value</param>
    internal void ChangeValue(
        DateTimeOffset now,
        string value)
    {
        Value = value;
        UpdatedAt = now;
    }

    /// <summary>
    /// Marks this attribute as deleted (soft delete).
    /// </summary>
    /// <param name="now">Deletion timestamp</param>
    internal void Delete(DateTimeOffset now)
    {
        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    /// <summary>
    /// Restores a previously deleted attribute.
    /// </summary>
    /// <param name="now">Restoration timestamp</param>
    /// <remarks>
    /// The attribute becomes visible again in product specifications.
    /// </remarks>
    internal void Restore(DateTimeOffset now)
    {
        IsDeleted = false;
        UpdatedAt = now;
        DeletedAt = null;
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