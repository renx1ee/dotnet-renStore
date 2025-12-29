namespace RenStore.Domain.Entities;

public class CountryEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string OtherName { get; set; } = string.Empty;
    public string NormalizedOtherName { get; set; } = string.Empty;
    public string NameRu { get; set; } = string.Empty;
    public string NormalizedNameRu { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string PhoneCode { get; set; } = string.Empty;
    public IEnumerable<CityEntity>? Cities { get; set; }
    public IEnumerable<AddressEntity>? Addresses { get; set; }
    public IEnumerable<ProductDetailEntity>? ProductDetails { get; set; }
}