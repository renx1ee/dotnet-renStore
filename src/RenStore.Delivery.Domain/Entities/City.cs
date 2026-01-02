namespace RenStore.Delivery.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string NameRu { get; set; } = string.Empty;
    public string NormalizedName { get; set; } = string.Empty;
    public string NormalizedNameRu { get; set; } = string.Empty;
    public int CountryId { get; set; }
    public Country? Country { get; set; }
    public IEnumerable<Address>? Addresses { get; set; }
}