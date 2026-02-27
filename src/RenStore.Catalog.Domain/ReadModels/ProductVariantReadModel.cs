using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class ProductVariantReadModel
{
    /// <summary>
    /// Unique identifier of the product variant.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Display the name of this specific variant.
    /// Length must be between 25 and 500 characters after trimming.
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Uppercase normalized version of the variant name for case-insensitive operations.
    /// Automatically derived from <see cref="Name"/>.
    /// </summary>
    public string NormalizedName { get; init; } 
    
    /// <summary>
    /// Internal article number that uniquely identifies this product variant.
    /// </summary>
    public long Article { get; init; }
    
    /// <summary>
    /// Current lifecycle state on this product variant.
    /// </summary>
    public ProductVariantStatus Status { get; init; }
    
    /// <summary>
    /// SEO-friendly URL slug for this product variant.
    /// Used to generate permanent links in the catalog and for search engine optimization.
    /// </summary>
    public string Url { get; init; }
    
    /// <summary>
    /// Image Unique Identifier //TODO:
    /// </summary>
    public Guid MainImageId { get; init; }
    
    /// <summary>
    /// Measurement system used for sizing in this product variant.
    /// Determines which size chart applies and how sizes are displayed to customers.
    /// </summary>
    public SizeSystem SizeSystem { get; init; }
    
    /// <summary>
    /// Category of sizing applicable to this product variant.
    /// Determines which size ranges and conversion tables are relevant.
    /// </summary>
    public SizeType SizeType { get; init; }
    
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
    /// Unique identifier of the product.
    /// </summary>
    public Guid ProductId { get; init; }
    
    /// <summary>
    /// Unique identifier of the color.
    /// </summary>
    public int ColorId { get; init; }
}