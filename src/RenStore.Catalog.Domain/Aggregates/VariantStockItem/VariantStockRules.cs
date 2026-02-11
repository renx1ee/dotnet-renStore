using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.StockItem;

internal static class VariantStockRules
{
    /// <summary>
    /// Validates inventory stock quantity.
    /// </summary>
    /// <param name="inStock">Stock quantity to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when quantity is negative
    /// </exception>
    /// <remarks>
    /// Prevents impossible stock levels. Zero stock is allowed (out of stock condition).
    /// </remarks>
    internal static void ValidateInStock(int inStock)
    {
        if(inStock < 0)
            throw new DomainException("InStock cannot be less then 0.");
    }

    internal static void ProductVariantIdValidate(Guid variantId)
    {
        if(variantId == Guid.Empty)
            throw new DomainException("Variant ID cannto be guid empty.");
    }
    
    internal static void AddToStockValidation(int count)
    {
        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
    }

    internal static void ReturnSoldValidation(int count, int sales)
    {
        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
        
        if (sales < count)
            throw new DomainException("Returned quantity exceeds sold quantity.");
    }

    internal static void RemoveFromStockCommonValidation(int count, int inStock)
    {
        if (count <= 0)
            throw new DomainException("Cannot sell 0 or less products.");
        
        if(count > inStock)
            throw new DomainException("The count of sells exceed available count.");
    }
}