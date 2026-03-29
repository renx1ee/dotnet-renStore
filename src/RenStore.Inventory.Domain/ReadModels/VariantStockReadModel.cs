using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Domain.ReadModels;

public sealed class VariantStockReadModel
{
    /// <summary>
    /// Unique identifier of the variant stock.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Available inventory quantity for this product variant.
    /// </summary>
    public int InStock { get; set; }
    
    /// <summary>
    /// Total number of unit sold for this product variant. 
    /// </summary>
    public int Sales { get; set; }
    
    public WriteOffReason? Reason { get; set; }
    
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// Date when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Date when the entity was updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid UpdatedById { get; set; } 
    
    public string UpdatedByRole { get; set; } 
    
    /// <summary>
    /// Unique identifier of the variant.
    /// </summary>
    public Guid VariantId { get; set; }
    
    /// <summary>
    /// Unique identifier of the variant size.
    /// </summary>
    public Guid SizeId { get; set; }
}