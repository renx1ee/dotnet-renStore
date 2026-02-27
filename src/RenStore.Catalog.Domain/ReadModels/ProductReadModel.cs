using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public sealed class ProductReadModel
{
    /// <summary>a
    /// Unique identifier of the product.
    /// </summary>
    public Guid Id { get; init; }
    
    /*/// <summary>
    /// Overall rating calculated of all product variants.
    /// </summary>
    public Rating OverallRating { get; private set; } */
    
    /// <summary>
    /// Current lifecycle status of the product.
    /// </summary>
    public ProductStatus Status { get; init; }
    
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
    /// Unique identifier of the seller.
    /// </summary>
    public long SellerId { get; init; }
    
    /// <summary>
    /// Unique identifier of the sub category.
    /// </summary>
    public Guid SubCategoryId { get; init; }
}