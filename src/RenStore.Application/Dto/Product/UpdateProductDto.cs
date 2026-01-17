using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Clothes;
using RenStore.Domain.Enums;

namespace RenStore.Application.Dto.Product;

public class UpdateProductDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public decimal OldPrice { get; set; }
    public string Description { get; set; }
    public uint InStock { get; set; }
    public string? ImagePath { get; set; }
    public string ImageMiniPath { get; set; }
    public List<string>? ImagesListPath { get; set; }
    public string CategoryName { get; set; }
    public uint SellerId { get; set; }
    public string Brend { get; set; }
    public string? CountryOfManufacture { get; set; }
    public string? ModelFeatures { get; set; } 
    public string? DecorativeElements { get; set; }
    public string? Equipment { get; set; }
    public uint? QuantityPerPackage { get; set; }
    public string? Composition { get; set; }
    public ColorStatus? Color { get; set; }
    public TypeOfPackaging? TypeOfPackaging { get; set; }
    public Neckline? Neckline { get; set; }
    public TheCut? TheCut { get; set; }
    public TypeOfPockets? TypeOfPockets { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public string? TakingCareOfThings { get; set; }
    public IEnumerable<ClothesSizes> Sizes { get; set; }
}