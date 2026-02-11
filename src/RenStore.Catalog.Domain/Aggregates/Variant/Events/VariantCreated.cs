using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/*/// <param name="InStock">Initial inventory quantity</param>*/
/// <summary>
/// Records the creation of a new product variant in the catalog.
/// Represents a specific configuration of a base product with unique attributes.
/// </summary>
/// <param name="OccurredAt">Timestamp when the variant was created</param>
/// <param name="VariantId">Unique identifier for the new variant</param>
/// <param name="ProductId">Identifier of the parent product</param>
/// <param name="ColorId">Color option identifier for this variant</param>
/// <param name="Name">Customer-facing display name</param>
/// <param name="SizeSystem">Measurement system for sizes (RU/US/EU)</param>
/// <param name="SizeType">Category of sizing (Clothes/Shoes)</param>
/// <param name="Url">SEO-friendly URL slug</param>
/// <remarks>
/// Variants allow customers to choose between different configurations of the same product.
/// Created variants start in unpublished state and require additional setup before going live.
/// </remarks>
public record VariantCreated(
    DateTimeOffset OccurredAt,
    Guid VariantId,
    Guid ProductId,
    int ColorId,
    string Name,
    /*int InStock,*/
    SizeSystem SizeSystem,
    SizeType SizeType,
    string Url);