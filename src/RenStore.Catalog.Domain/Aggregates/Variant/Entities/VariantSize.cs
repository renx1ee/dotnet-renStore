using RenStore.Catalog.Domain.ValueObjects;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Entities;

public sealed class VariantSize
{
    private readonly List<PriceHistory> _prices = new();
    
    public Guid Id { get; private set; }
    public Size Size { get; private set; } 
    public bool IsDeleted { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public Guid VariantId { get; private set; }
    
    /// <summary>
    /// The collection of price history associated with this variant.
    /// </summary>
    public IReadOnlyList<PriceHistory> Prices => _prices.AsReadOnly();
    
    private VariantSize() { }
    
    internal static VariantSize Create(
        Guid id,
        Size size,
        Guid variantId,
        DateTimeOffset now)
    {
        return new VariantSize()
        {
            Id = id,
            VariantId = variantId,
            CreatedAt = now,
            Size = size,
            IsDeleted = false
        };
    }

    internal void AddPrice(
        DateTimeOffset now,
        PriceHistory newPrice)
    {
        _prices.Add(newPrice);
        
        UpdatedAt = now;
    }

    internal void CloseCurrentPrice(DateTimeOffset now)
    {
        var activePrices = _prices.Where(x => x.IsActive).ToList();
        
        if (activePrices.Any())
        {
            foreach (var item in activePrices)
                item.Deactivate(now);
            
            UpdatedAt = now;
        }
    }
    
    internal void Delete(DateTimeOffset now)
    {
        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
    
    internal void Restore(DateTimeOffset now)
    {
        IsDeleted = false;
        UpdatedAt = now;
        DeletedAt = null;
    }
    
    // TODO: need to check if the Size already exists
    private void ChangeSize(
        DateTimeOffset now,
        Size size)
    {
        if(Size == size) return;

        Size = size;
        UpdatedAt = now;
    }
}

/*
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
           IsAvailable = isAvailable,
           ProductVariantId = productClothId,
           IsDeleted = isDeleted,
           CreatedAt = createdAt,
           UpdatedAt = updatedAt,
           DeletedAt = deletedAt
       };
   }
 */