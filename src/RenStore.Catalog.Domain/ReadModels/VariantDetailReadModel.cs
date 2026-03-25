using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Domain.ReadModels;

public class VariantDetailReadModel
{
    public Guid Id { get; set; }
    public string Description { get; set; } 
    public string Composition { get; set; }
    public string? ModelFeatures { get; set; }
    public string? DecorativeElements { get; set; } 
    public string? Equipment { get; set; }
    public string? CaringOfThings { get; set; } 
    public TypeOfPacking? TypeOfPacking { get; set; }
    public Guid UpdatedById { get; set; } 
    public string UpdatedByRole { get; set; } 
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public int CountryOfManufactureId { get; set; }
    public Guid VariantId { get; set; }
}