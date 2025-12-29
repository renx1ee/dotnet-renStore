namespace RenStore.Domain.Entities;

public class CityEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameRu { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string NormalizedNameRu { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public CountryEntity? Country { get; set; }
    public IEnumerable<AddressEntity>? Addresses { get; set; }
}