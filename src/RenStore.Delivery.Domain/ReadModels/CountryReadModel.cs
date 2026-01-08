using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.ReadModels;

/// <summary>
/// Read model, represents the country entity.
/// Used to display and transmit data without state change logic.
/// </summary>
public class CountryReadModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string NormalizedName { get; init; } = string.Empty;
    public string NameRu { get; init; } = string.Empty;
    public string NormalizedNameRu { get; init; } = string.Empty;
    /// <summary>
    /// ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string Code { get; init; } = string.Empty;
    public string PhoneCode { get; init; } = string.Empty;
    public bool IsDeleted { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; }
    public DateTimeOffset? DeletedAt { get; init; }
    public IReadOnlyCollection<Address>? Addresses { get; set; }
    public IReadOnlyCollection<City>? Cities { get; set; }
}