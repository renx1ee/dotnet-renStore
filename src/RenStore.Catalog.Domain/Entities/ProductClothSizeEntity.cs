using RenStore.Catalog.Domain.Enums.Clothes;

namespace RenStore.Catalog.Domain.Entities;

public class ProductClothSizeEntity
{
    public Guid Id { get; private set; }
    public ClothesSizes? ClothSize { get; private set; }
    public int Amount { get; private set; }
    public Guid ProductClothId { get; private set; }
    public ProductClothEntity? ProductCloth { get; private set; }
}