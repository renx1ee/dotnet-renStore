using RenStore.Inventory.Domain.Aggregates.Stock.Events;
using RenStore.Inventory.Domain.Aggregates.Stock.Rules;
using RenStore.Inventory.Domain.Enums;
using RenStore.SharedKernal.Domain.Common;
using RenStore.SharedKernal.Domain.Exceptions;

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
    
    public bool IsDeleted { get; private set; }
    
    public WriteOffReason? Reason { get; private set; }
    
    /// <summary>
    /// Date when the entity was created.
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }
    
    /// <summary>
    /// Date when the entity was updated.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; private set; }
    
    public DateTimeOffset? DeletedAt { get; private set; }
    
    public Guid UpdatedById { get; private set; } 
    
    public string UpdatedByRole { get; private set; } 
    
    /// <summary>
    /// Unique identifier of the variant.
    /// </summary>
    public Guid VariantId { get; private set; }
    
    /// <summary>
    /// Unique identifier of the variant size.
    /// </summary>
    public Guid SizeId { get; private set; }
    
    private VariantStock() { }

    public bool CanDecrease(int count) => InStock >= count;

    public static VariantStock Create(
        DateTimeOffset now,
        Guid sizeId,
        Guid variantId,
        int initialStock)
    {
        VariantStockRules.ValidateInStock(initialStock);
        VariantStockRules.ProductVariantIdValidate(variantId);
        VariantStockRules.ProductVariantIdValidate(sizeId);

        var stockId = Guid.NewGuid();
        var variant = new VariantStock();
        
        variant.Raise(new StockCreatedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            SizeId: sizeId,
            VariantId: variantId,
            StockId: stockId,
            InitialStock: initialStock));

        return variant;
    }
    
    public void AddToStock(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();
        
        VariantStockRules.AddToStockValidation(InStock, count);

        Raise(new StockAddedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            StockId: Id,
            Count: count));
    }
    
    public void StockWriteOff(
        DateTimeOffset now,
        WriteOffReason reason,
        int count)
    {
        EnsureNotDeleted();
        
        VariantStockRules.RemoveFromStockCommonValidation(count, InStock);
        VariantStockRules.ChangeCountValidate(count);

        Raise(new StockWrittenOffEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            StockId: Id,
            Reason: reason,
            Count: count));
    }
    
    public void Sell(
        DateTimeOffset now,
        int count)
    {
        EnsureNotDeleted();
        
        VariantStockRules.RemoveFromStockCommonValidation(count, InStock);
        VariantStockRules.ChangeCountValidate(count);
        
        Raise(new StockSoldEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            StockId: Id,
            Count: count));
    }
    
    public void ReturnSale(
        int count,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        VariantStockRules.ReturnSoldValidation(count, Sales);
        VariantStockRules.ChangeCountValidate(count);
        
        Raise(new StockSaleReturnedEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            StockId: Id,
            Count: count));
    }
    
    public void SetStock(
        DateTimeOffset now,
        int newStock)
    {
        EnsureNotDeleted();
        
        VariantStockRules.InStockValidate(newStock);
        VariantStockRules.ChangeCountValidate(newStock);
        
        if(InStock == newStock) return;
        
        Raise(new StockSetEvent(
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            StockId: Id,
            NewStock: newStock));
    }
    
    public void Decrease(
        int count,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
    
        VariantStockRules.RemoveFromStockCommonValidation(count, InStock);
        VariantStockRules.ChangeCountValidate(count);
    
        Raise(new StockDecreasedEvent(
            EventId:   Guid.NewGuid(),
            OccurredAt: now,
            StockId:   Id,
            Count:     count));
    }

    public void ReturnReservation(
        int count,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
    
        VariantStockRules.ChangeCountValidate(count);
    
        Raise(new StockReservationReturnedEvent(
            EventId:    Guid.NewGuid(),
            OccurredAt: now,
            StockId:    Id,
            Count:      count));
    }
    
    public void Delete(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        EnsureNotDeleted();
        
        VariantStockRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        Raise(new StockSoftDeletedEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            SizeId: SizeId,
            VariantId: VariantId,
            StockId: Id));
    }

    public void Restore(
        Guid updatedById,
        string updatedByRole,
        DateTimeOffset now)
    {
        if(!IsDeleted)
        {
            throw new DomainException(
                "Stock was not deleted.");
        }
        
        VariantStockRules.UpdatedByParametersValidation(
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        Raise(new StockRestoredEvent(
            UpdatedById: updatedById,
            UpdatedByRole: updatedByRole,
            EventId: Guid.NewGuid(), 
            OccurredAt: now,
            SizeId: SizeId,
            VariantId: VariantId,
            StockId: Id));
    }
    
    protected override void Apply(IDomainEvent @event)
    {
        switch (@event)
        {
            case StockCreatedEvent e:
                Id = e.StockId;
                CreatedAt = e.OccurredAt;
                SizeId = e.SizeId;
                VariantId = e.VariantId;
                InStock = e.InitialStock;
                Sales = 0;
                break;
            
            case StockAddedEvent e:
                InStock += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockWrittenOffEvent e:
                InStock -= e.Count;
                UpdatedAt = e.OccurredAt;
                Reason = e.Reason;
                break;
            
            case StockSoldEvent e:
                Sales += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockSaleReturnedEvent e:
                InStock += e.Count;
                Sales -= e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockSetEvent e:
                InStock = e.NewStock;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockDecreasedEvent e:
                InStock -= e.Count;
                UpdatedAt = e.OccurredAt;
                break;

            case StockReservationReturnedEvent e:
                InStock += e.Count;
                UpdatedAt = e.OccurredAt;
                break;
            
            case StockSoftDeletedEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                DeletedAt = e.OccurredAt;
                IsDeleted = true;
                break;
            
            case StockRestoredEvent e:
                UpdatedById = e.UpdatedById;
                UpdatedByRole = e.UpdatedByRole;
                UpdatedAt = e.OccurredAt;
                DeletedAt = null;
                IsDeleted = false;
                break;
        }
    }

    public static VariantStock Rehydrate(IEnumerable<IDomainEvent> history)
    {
        var stock = new VariantStock();

        foreach (var @event in history)
        {
            stock.Apply(@event);
            stock.Version++;
        }

        return stock;
    }
    
    private void EnsureNotDeleted(string? message = null)
    {
        if (IsDeleted)
        {
            throw new DomainException(
                message ?? "Entity is deleted.");
        }
    }
}