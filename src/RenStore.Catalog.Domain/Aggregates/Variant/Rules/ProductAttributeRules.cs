using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Rules;

/// <summary>
/// Business rules and validation logic for product attributes.
/// Ensures data integrity and enforces domain constraints on attribute creation and modification.
/// </summary>
internal static class ProductAttributeRules
{
    /// <summary>
    /// Validates a product variant identifier.
    /// </summary>
    /// <param name="productVariantId">Variant identifier to validate</param>
    /// <exception cref="DomainException">
    /// Thrown when identifier is <see cref="Guid.Empty"/>
    /// </exception>
    /// <remarks>
    /// Ensures attributes are always linked to a valid parent variant.
    /// Empty GUIDs indicate uninitialized or invalid relationships.
    /// </remarks>
    internal static void ProductVariantIdNormalizeAndValidate(Guid productVariantId)
    {
        if(productVariantId == Guid.Empty)
            throw new DomainException($"ProductVariantId cannot be guid empty.");
    }
}