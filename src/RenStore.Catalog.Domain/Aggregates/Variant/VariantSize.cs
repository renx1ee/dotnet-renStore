using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

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
    public void ChangeSize(
        DateTimeOffset now,
        Size size)
    {
        EnsureNotDeleted();
        
        if(Size == size) return;

        Size = size;
        UpdatedAt = now;
    }
    
    public void SetStock(
        DateTimeOffset now,
        int newStock)
    {
        EnsureNotDeleted();
        
        VariantSizeRules.InStockValidate(newStock);
        
        if(InStock == newStock) return;

        IsAvailable = newStock > 0;
        InStock = newStock;
        UpdatedAt = now;
    }
    
    public void AddToStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();
        
        VariantSizeRules.ChangeCountValidate(count);

        var newStock = InStock + count;
        
        VariantSizeRules.InStockValidate(newStock);

        InStock = newStock;
        IsAvailable = InStock > 0;
        UpdatedAt = now;
    }
    
    public void RemoveFromStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();

        VariantSizeRules.ChangeCountValidate(count);
        
        if(count > InStock)
            throw new DomainException("The count of sells exceed available count.");

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
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
            throw new DomainException(message ?? "Entity is deleted.");
    }
}