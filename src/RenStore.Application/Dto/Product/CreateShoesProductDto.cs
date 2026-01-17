using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Shoes;
using RenStore.Domain.Enums;

namespace RenStore.Application.Dto.Product;

public class CreateShoesProductDto : CreateProductBaseDto
{
    public FullnessOfShoes? FullnessOfShoes { get; set; }
    public ShoeInsoleMaterial? ShoeInsoleMaterial { get; set; }
    public ShoeLiningMaterial? ShoeLiningMaterial { get; set; }
    public ShoeSoleMaterial? ShoeSoleMaterial { get; set; }
    public SoleFasteningMethod? SoleFasteningMethod { get; set; }
    public TypeOfFastener? TypeOfFastener { get; set; }
    public uint? SoleHeight { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public string? TakingCareOfThings { get; set; }
}