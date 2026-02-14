using RenStore.Inventory.Domain.Aggregates.Stock.Events;
using RenStore.Inventory.Domain.Aggregates.Stock.Rules;
using RenStore.Inventory.Domain.Enums;

namespace RenStore.Inventory.Domain.Aggregates.Stock;
// TODO: unique index в БД на VariantId: unique index в БД на VariantId
/// <summary>
/// Represents a variant stock physical entity with lifecycle and invariants.
/// </summary>
public sealed class VariantStock 
    : RenStore.SharedKernal.Domain.Common.AggregateRoot
{
    /// <summary>
    /// Unique identifier of the variant stock.
    /// </summary>
    public Guid Id { get; private set; }
    
    /// <summary>
    /// Available inventory quantity for this product variant.
    /// </summary>
    public int InStock { get; private set; }
    
    /// <summary>
    /// Total number of unit sold for this product variant. 
    /// </summary>
    /// <remarks>
    /// Include on each successful order.
    /// Does not include cancelled or returned items. // TODO:
    /// </remarks>
    public int Sales { get; private set; }
    
    /// <summary>
    /// Date when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    
    /// <summary>
    /// Date when the entity was updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }
    
    /// <summary>
    /// Unique identifier of the variant size.
    /// </summary>
    public Guid SizeId { get; private set; }
    
    private VariantStock() { }

    public bool CanDecrease(int count) => InStock >= count;

    public static VariantStock Create(
        DateTimeOffset now,
        Guid variantId,
        int initialStock)
    {
        VariantStockRules.ValidateInStock(initialStock);

        VariantStockRules.ProductVariantIdValidate(variantId);

        var stockId = Guid.NewGuid();
        var variant = new VariantStock();
        
        variant.Raise(new StockCreated(
            OccurredAt: now,
            SizeId: variantId,
            StockId: stockId,
            InitialStock: initialStock));

        return variant;
    }
    
    public void AddToStock(
        DateTimeOffset now,
        int count)
    {
        // TODO: разобраться с валидацией
        VariantStockRules.InStockValidate(count);
        VariantStockRules.AddToStockValidation(count);

        Raise(new StockAdded(
            OccurredAt: now,
            StockId: Id,
            Count: count));
    }
    
    public void StockWriteOff(
        DateTimeOffset now,
        WriteOffReason reason,
        int count)
    {
        // TODO: разобраться с валидацией
        VariantStockRules.RemoveFromStockCommonValidation(count, InStock);
        VariantStockRules.ChangeCountValidate(count);

        Raise(new StockWrittenOff(
            OccurredAt: now,
            StockId: Id,
            Reason: reason,
            Count: count));
    }
    
    public void Sell(
        DateTimeOffset now,
        int count)
    {
        // TODO: разобраться с валидацией
        VariantStockRules.RemoveFromStockCommonValidation(count, InStock);
        VariantStockRules.ChangeCountValidate(count);
        
        Raise(new StockSold(
            OccurredAt: now,
            StockId: Id,
            Count: count));
    }
    
    public void ReturnSale(
        int count,
        DateTimeOffset now)
    {
        // TODO: разобраться с валидацией
        VariantStockRules.ReturnSoldValidation(count, Sales);
        VariantStockRules.ChangeCountValidate(count);
        
        Raise(new StockSaleReturned(
            OccurredAt: now,
            StockId: Id,
            Count: count));
    }
    
    public void SetStock(
        DateTimeOffset now,
        Guid sizeId,
        int newStock)
    {
        // TODO: разобраться с валидацией
        VariantStockRules.InStockValidate(newStock);
        VariantStockRules.ChangeCountValidate(newStock);
        
        if(InStock == newStock) return;
        
        Raise(new StockSet(
            OccurredAt: now,
            SizeId: Id,
            VariantSizeId: sizeId,
            NewStock: newStock));
    }
    
    protected override void Apply(object @event)
    {
        switch (@event)
        {
            case StockCreated e:
                Id = e.StockId;
                CreatedAt = e.OccurredAt;
                SizeId = e.SizeId;
                InStock = e.InitialStock;
                Sales = 0;
                break;
            
            case StockAdded e:
                InStock += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockWrittenOff e:
                InStock -= e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockSold e:
                InStock -= e.Count;
                Sales += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockSaleReturned e:
                InStock += e.Count;
                Sales -= e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockSet e:
                InStock = e.NewStock;
                UpdatedAt = e.OccurredAt;
                break;
        }
    }
}