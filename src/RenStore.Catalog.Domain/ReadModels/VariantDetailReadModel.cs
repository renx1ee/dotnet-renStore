using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public class VariantDetailReadModel
{
    public Guid Id { get; init; }
    public string Description { get; init; } 
    public string Composition { get; init; }
    public string? ModelFeatures { get; init; }
    public string? DecorativeElements { get; init; } 
    public string? Equipment { get; init; }
    public string? CaringOfThings { get; init; } 
    public TypeOfPacking? TypeOfPacking { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public int CountryOfManufactureId { get; init; }
    public Guid VariantId { get; init; }
}