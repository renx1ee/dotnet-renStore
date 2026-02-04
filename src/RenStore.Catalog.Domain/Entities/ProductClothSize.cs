using RenStore.Catalog.Domain.Enums.Clothes;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Entities;

/// <summary>
/// Represents a product cloth size physical entity with lifecycle and invariants.
/// </summary>
public class ProductClothSize
    : RenStore.Catalog.Domain.Entities.EntityWithSoftDeleteBase
{
    private ProductCloth? _productCloth;
    
    public Guid Id { get; private set; }
    public ClothesSizes? ClothSize { get; private set; }
    public int InStock { get; private set; }
    public bool IsAvailable { get; private set; }
    public Guid ProductClothId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private const int MaxInStock = 100000;
    private const int MinInStock = 0;
    
    internal static ProductClothSize Create(
        ClothesSizes clothesSizes,
        int inStock,
        Guid productClothId,
        DateTimeOffset now)
    {
        ProductClothIdValidate(productClothId);

        InStockValidate(inStock);
        
        var size = new ProductClothSize()
        {
            ProductClothId = productClothId,
            InStock = inStock,
            ClothSize = clothesSizes,
            CreatedAt = now,
            IsDeleted = false
        };

        if (inStock > MinInStock)
            size.IsAvailable = true;
        else
            size.IsAvailable = false;

        return size;
    }
    
    public static ProductClothSize Reconstitute(
        Guid id,
        ClothesSizes size,
        int amount,
        bool isAvailable,
        Guid productClothId,
        bool isDeleted,
        DateTimeOffset createdAt,
        DateTimeOffset? updatedAt,
        DateTimeOffset? deletedAt)
    {
        var clothSize = new ProductClothSize()
        {
            Id = id,
            ClothSize = size,
            InStock = amount,
            IsAvailable = isAvailable,
            ProductClothId = productClothId,
            IsDeleted = isDeleted,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt,
            DeletedAt = deletedAt
        };

        return clothSize;
    }
    
    public void ChangeClothesSizes(
        DateTimeOffset now,
        ClothesSizes size)
    {
        EnsureNotDeleted();
        
        if(ClothSize == size) return;

        ClothSize = size;
        UpdatedAt = now;
    }

    public void SetStock(
        DateTimeOffset now,
        int newStock)
    {
        EnsureNotDeleted();
        
        InStockValidate(newStock);
        
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
        
        ChangeCountValidate(count);

        var newStock = InStock + count;
        
        InStockValidate(newStock);

        InStock = newStock;
        IsAvailable = InStock > 0;
        UpdatedAt = now;
    }
    
    public void RemoveFromStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();

        ChangeCountValidate(count);
        
        if(count > InStock)
            throw new DomainException("The count of sells exceed available count.");

        InStock -= count;
        
        IsAvailable = InStock > 0;
        UpdatedAt = now;
    }
    
    private static void ProductClothIdValidate(Guid productClothId)
    {
        if(productClothId == Guid.Empty)
            throw new DomainException("Product Cloth Id cannot be guid empty.");
    }
    
    private static void InStockValidate(int amount)
    {
        if(amount is > MaxInStock or < MinInStock)
            throw new DomainException($"Product Cloth Size amount must be between {MinInStock} and {MaxInStock}.");
    }
    
    private static void ChangeCountValidate(int amount)
    {
        if(amount is > MaxInStock or < 1)
            throw new DomainException($"Product Cloth Size count must be between {1} and {MaxInStock}.");
    }
}