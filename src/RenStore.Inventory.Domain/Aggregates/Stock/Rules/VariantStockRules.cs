using RenStore.Inventory.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Inventory.Domain.Aggregates.Stock.Rules;

internal static class VariantStockRules
{
    /// <summary>
    /// Validates inventory quantity for a size option.
    /// </summary>
    /// <param name="amount">Inventory quantity to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when quantity is outside allowed range (0-100,000)
    /// </exception>
    /// <remarks>
    /// Prevents unrealistic inventory levels while allowing for large stock quantities.
    /// Zero stock is valid (size exists but is out of stock).
    /// </remarks>
    internal static void InStockValidate(int amount)
    {
        if(amount is > InventoryConstants.VariantStock.MaxInventoryStockCount 
                  or < InventoryConstants.VariantStock.MinInventoryStockCount)
        {
            throw new DomainException(
                $"Variant Size amount must be between " +
                $"{InventoryConstants.VariantStock.MaxInventoryStockCount} and " +
                $"{InventoryConstants.VariantStock.MinInventoryStockCount}.");
        }
    }
    
    /// <summary>
    /// Validates a stock change amount (addition or removal).
    /// </summary>
    /// <param name="amount">Change amount to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when change amount is outside allowed range (1-100,000)
    /// </exception>
    /// <remarks>
    /// Used for inventory adjustments where a change must be non-zero.
    /// Different from InStockValidate as it validates delta changes, not absolute levels.
    /// </remarks>
    internal static void ChangeCountValidate(int amount)
    {
        if(amount is > InventoryConstants.VariantStock.MaxInventoryStockCount or < 1)
        {
            throw new DomainException(
                $"Variant Size count must be between " +
                $"{InventoryConstants.VariantStock.MinInventoryStockCount} and " +
                $"{InventoryConstants.VariantStock.MaxInventoryStockCount}.");
        }
    }
    
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
            throw new DomainException(
                "InStock cannot be less then 0.");
    }

    internal static void ProductVariantIdValidate(Guid variantId)
    {
        if(variantId == Guid.Empty)
            throw new DomainException(
                "Variant ID cannot be guid empty.");
    }
    
    internal static void AddToStockValidation(int count)
    {
        if (count <= 0)
            throw new DomainException(
                "Cannot sell 0 or less products.");
    }

    internal static void ReturnSoldValidation(int count, int sales)
    {
        if (count <= 0)
            throw new DomainException(
                "Cannot sell 0 or less products.");
        
        if (sales < count)
            throw new DomainException(
                "Returned quantity exceeds sold quantity.");
    }

    internal static void RemoveFromStockCommonValidation(int count, int inStock)
    {
        if (count <= 0)
            throw new DomainException(
                "Cannot sell 0 or less products.");
        
        if(count > inStock)
            throw new DomainException(
                "The count of sells exceed available count.");
    }
    
    internal static void UpdatedByParametersValidation(
        Guid updatedById,
        string updatedByRole)
    {
        if (updatedById == Guid.Empty)
            throw new DomainException(
                "Updated By ID cannot be empty guid.");

        if (string.IsNullOrWhiteSpace(updatedByRole))
            throw new DomainException(
                "Updated By role cannot be empty string.");
    }
}