using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Domain.Enums.Clothes;
using RenStore.Domain.Enums;

namespace RenStore.Application.Dto.Product;

public class CreateClothesProductDto  : CreateProductBaseDto
{
    public Neckline? Neckline { get; set; }
    public TheCut? TheCut { get; set; }
    public TypeOfPockets? TypeOfPockets { get; set; }
    public Gender? Gender { get; set; }
    public Season? Season { get; set; }
    public string? TakingCareOfThings { get; set; }
    public IEnumerable<ClothesSizes> Sizes { get; set; }
}