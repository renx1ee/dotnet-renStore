using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.ReadModels;

/// <summary>
/// Read model, represents the city entity.
/// Used to display and transmit data without state change logic.
/// </summary>
public class CityReadModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NameRu { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
    public string NormalizedNameRu { get; init; } = string.Empty;
    public bool IsDeleted { get; init; } = false;
    public int CountryId { get; init; }
    public Country? Country { get; init; }
    public IEnumerable<Address>? Addresses { get; init; }
}