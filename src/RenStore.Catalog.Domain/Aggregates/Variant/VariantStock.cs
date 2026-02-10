using RenStore.Catalog.Domain.Aggregates.Variant.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

internal sealed class VariantStock
{
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
    
    private VariantStock() { }

    internal static VariantStock Create(int initialStock)
    {
        return new VariantStock()
        {
            InStock = initialStock,
            Sales = 0
        };
    }
    
    internal void ValidateAdd(int count)
    {
        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
    }
    
    internal void ValidateRemove(int count)
    {
        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
        
        if(count > InStock)
            throw new DomainException("The count of sells exceed available count.");
    }
    
    internal void ValidateSell(int count)
    {
        ValidateRemove(count);
    }
    
    // TODO: 
    internal void ValidateReturnSell(int count)
    {
        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
    }

    internal void Apply(VariantStockAdded e)
    {
        InStock += e.Count;
    }
    
    internal void Apply(VariantStockWrittenOff e)
    {
        InStock -= e.Count;
    }
    
    internal void Apply(VariantSold e)
    {
        InStock -= e.Count;
        Sales += e.Count;
    }
    
    internal void Apply(VariantSaleReturned e)
    {
        InStock += e.Count;
        Sales -= e.Count;
    }
}