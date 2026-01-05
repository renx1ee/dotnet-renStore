namespace RenStore.Delivery.Application.Features.Address.Queries;

public sealed record AddressSearchCriteria
{
    public int? CountryId { get; set; }
    public int? CityId { get; set; }
    public string? UserId { get; set; } = string.Empty;
    public string? Street { get; set; } = string.Empty;
    public string? BuildingNumber { get; set; } = string.Empty;
}