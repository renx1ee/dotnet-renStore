using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class ProductVariantReadModel
{
    /// <summary>
    /// Unique identifier of the product variant.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Display the name of this specific variant.
    /// Length must be between 25 and 500 characters after trimming.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Uppercase normalized version of the variant name for case-insensitive operations.
    /// Automatically derived from <see cref="Name"/>.
    /// </summary>
    public string NormalizedName { get; set; } 
    
    /// <summary>
    /// Internal article number that uniquely identifies this product variant.
    /// </summary>
    public long Article { get; set; }
    
    /// <summary>
    /// Current lifecycle state on this product variant.
    /// </summary>
    public ProductVariantStatus Status { get; set; }
    
    /// <summary>
    /// SEO-friendly URL slug for this product variant.
    /// Used to generate permanent links in the catalog and for search engine optimization.
    /// </summary>
    public string Url { get; set; }
    
    /// <summary>
    /// Image Unique Identifier
    /// </summary>
    public Guid MainImageId { get; set; }
    
    /// <summary>
    /// Measurement system used for sizing in this product variant.
    /// Determines which size chart applies and how sizes are displayed to customers.
    /// </summary>
    public SizeSystem SizeSystem { get; set; }
    
    /// <summary>
    /// Category of sizing applicable to this product variant.
    /// Determines which size ranges and conversion tables are relevant.
    /// </summary>
    public SizeType SizeType { get; set; }
    
    public Guid? UpdatedById { get; set; } 
    public string? UpdatedByRole { get; set; } 
    
    /// <summary>
    /// Date when the product was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the product was updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
    
    /// <summary>
    /// Date when the product was deleted.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
    
    /// <summary>
    /// Unique identifier of the size.
    /// </summary>
    public Guid ProductId { get; set; }
    
    /// <summary>
    /// Unique identifier of the color.
    /// </summary>
    public int ColorId { get; set; }
    
    /*public bool HasDiscount { get; set; }*/
    
    // denormalization fields
    
    public int? DiscountPercents { get; set; }
    
    public bool? SellerIsVerified { get; set; }
    
    public int? InStock { get; set; }
    
    public int? ReviewsCount { get; set; }
    
    public double? AverageRating { get; set; }
    
    public int? SalesCount { get; set; }
}