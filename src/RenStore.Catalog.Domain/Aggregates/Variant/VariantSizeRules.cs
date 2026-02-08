using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

/// <summary>
/// Business rules and validation logic for variant size options.
/// Manages inventory constraints and availability for specific size configurations.
/// </summary>
internal static class VariantSizeRules
{
    private const int MaxInStock = 100000;
    private const int MinInStock = 0;
    
    /// <summary>
    /// Determines if a specific size is available based on inventory levels.
    /// </summary>
    /// <param name="inStock">Current inventory quantity for the size</param>
    /// <returns>True if inventory is greater than zero, otherwise false</returns>
    /// <remarks>
    /// Used to control purchase availability for individual size options.
    /// Size-specific availability allows customers to see which sizes are in stock.
    /// </remarks>
    internal static bool SizeIsAvailable(int inStock)
    {
        if (inStock > MinInStock) 
            return true;
    
        return false;
    }
    
    /// <summary>
    /// Validates a product variant identifier for size association.
    /// </summary>
    /// <param name="productVariantId">Variant identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is <see cref="Guid.Empty"/>
    /// </exception>
    /// <remarks>
    /// Ensures size options are properly linked to a parent product variant.
    /// Note: Parameter name suggests it might have been renamed from productVariantId.
    /// </remarks>
    internal static void ProductVariantIdValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException("Variant size Id cannot be guid empty.");
    }
    
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
        if(amount is > MaxInStock or < MinInStock)
            throw new DomainException($"Variant Size amount must be between {MinInStock} and {MaxInStock}.");
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
        if(amount is > MaxInStock or < 1)
            throw new DomainException($"Variant Size count must be between {1} and {MaxInStock}.");
    }
}