using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.Aggregates.Variant.Events;

/// <summary>
/// Records the addition of detailed technical and descriptive information to a product variant.
/// Provides comprehensive specifications for customer evaluation and regulatory compliance.
/// </summary>
/// <param name="OccurredAt">Timestamp when details were added</param>
/// <param name="Id">Unique identifier for the details record</param>
/// <param name="VariantId">Identifier of the described product variant</param>
/// <param name="CountryOfManufactureId">Country where the product was manufactured</param>
/// <param name="Description">Full product description for customers</param>
/// <param name="Composition">Material makeup and percentages</param>
/// <param name="ModelFeatures">Key features and specifications</param>
/// <param name="DecorativeElements">Design and decorative components</param>
/// <param name="Equipment">Included accessories and packaging contents</param>
/// <param name="CaringOfThings">Care and maintenance instructions</param>
/// <param name="TypeOfPackaging">Packaging type classification</param>
/// <remarks>
/// Required for product publication. Details help customers make informed purchase decisions
/// and ensure compliance with labeling and safety regulations.
/// Optional fields allow for variant-specific information where applicable.
/// </remarks>
public record VariantDetailsCreated(
    DateTimeOffset OccurredAt,
    Guid Id,
    Guid VariantId,
    int CountryOfManufactureId,
    string Description,
    string Composition,
    string? ModelFeatures,
    string? DecorativeElements,
    string? Equipment,
    string? CaringOfThings,
    TypeOfPackaging? TypeOfPackaging);