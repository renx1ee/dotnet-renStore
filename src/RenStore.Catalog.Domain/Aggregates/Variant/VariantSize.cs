using RenStore.Catalog.Domain.Aggregates.Variant.Rules;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Represents a variant size physical entity with lifecycle and invariants.
/// </summary>
public class VariantSize
{
    private ProductVariant? _productVariant;
    
    public Guid Id { get; private set; }
    public int InStock { get; private set; }
    public Size Size { get; private set; } // TODO:
    public SizeSystem SizeSystem { get; private set; }
    public bool IsAvailable { get; private set; }
    public Guid ProductVariantId { get; private set; }
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    
    public static VariantSize Reconstitute(
        Guid id,
        Size size,
        SizeSystem sizeSystem, // TODO:
        int amount,
        bool isAvailable,
        Guid productClothId,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        DateTimeOffset? deletedAt)
    {
        return new VariantSize()
        {
            Id = id,
            SizeSystem = sizeSystem,
            Size = size,
            InStock = amount,
            IsAvailable = isAvailable,
            ProductVariantId = productClothId,
            IsDeleted = isDeleted,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };
    }
    
    internal static VariantSize Create(
        Size size,
        int inStock,
        Guid variantId,
        DateTimeOffset now)
    {
        return new VariantSize()
        {
            ProductVariantId = variantId,
            InStock = inStock,
            CreatedAt = now,
            Size = size,
            IsAvailable = VariantSizeRules.SizeIsAvailable(inStock),
            IsDeleted = false
        };
    }
    // TODO: need to check if the Size already exists
    internal void ChangeSize(
        DateTimeOffset now,
        Size size)
    {
        if(Size == size) return;

        Size = size;
        UpdatedAt = now;
    }
    
    internal void SetStock(
        DateTimeOffset now,
        int newStock)
    {
        IsAvailable = newStock > 0;
        InStock = newStock;
        UpdatedAt = now;
    }
    
    internal void AddToStock(
        DateTimeOffset now,
        int count)
    {
        InStock += count;
        IsAvailable = InStock > 0;
        UpdatedAt = now;
    }
    
    internal void RemoveFromStock(
        DateTimeOffset now,
        int count)
    {
        InStock -= count;
        
        IsAvailable = InStock > 0;
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
}