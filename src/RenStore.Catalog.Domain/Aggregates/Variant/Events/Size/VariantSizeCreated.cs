using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events.Size;

/// <summary>
/// Records the addition of a new size option with inventory to a product variant.
/// Enables customers to select specific sizes with independent stock tracking.
/// </summary>
/// <param name="OccurredAt">Timestamp when the size was added</param>
/// <param name="VariantId">Identifier of the product variant</param>
/// <param name="VariantSizeId">Unique identifier for this size option</param>
/// <param name="InStock">Initial inventory quantity for this specific size</param>
/// <param name="LetterSize">Alphanumeric size designation (e.g., "M", "10", "42")</param>
/// <param name="SizeSystem">Measurement system (RU/US/EU)</param>
/// <param name="SizeType">Category (Clothes/Shoes)</param>
/// <remarks>
/// Each size maintains independent stock levels within the same variant.
/// Size must be compatible with the variant's SizeType and SizeSystem.
/// Duplicate sizes within the same variant are not allowed.
/// </remarks>
public record VariantSizeCreated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid VariantSizeId,
    int InStock,
    LetterSize LetterSize,
    SizeSystem SizeSystem,
    SizeType SizeType);