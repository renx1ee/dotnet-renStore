using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant;

internal static class VariantSizeRules
{
    private const int MaxInStock = 100000;
    private const int MinInStock = 0;

    internal static bool SizeIsAvailable(int inStock)
    {
        if (inStock > MinInStock) 
            return true;
    
        return false;
    }
    
    internal static void ProductVariantIdValidate(Guid productClothId)
    {
        if(productClothId == Guid.Empty)
            throw new DomainException("Variant size Id cannot be guid empty.");
    }
    
    internal static void InStockValidate(int amount)
    {
        if(amount is > MaxInStock or < MinInStock)
            throw new DomainException($"Variant Size amount must be between {MinInStock} and {MaxInStock}.");
    }
    
    internal static void ChangeCountValidate(int amount)
    {
        if(amount is > MaxInStock or < 1)
            throw new DomainException($"Variant Size count must be between {1} and {MaxInStock}.");
    }
}