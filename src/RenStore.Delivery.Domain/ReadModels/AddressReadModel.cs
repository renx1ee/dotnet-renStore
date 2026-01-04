namespace RenStore.Delivery.Domain.ReadModels;

public class AddressReadModel
{
    public Guid Id { get; init; }
    public string HouseCode { get; init; } = string.Empty;
    public string Street { get; init; } = string.Empty;
    public string BuildingNumber  { get; init; } = string.Empty;
    public string ApartmentNumber { get; init; } = string.Empty;
    public string Entrance { get; init; } = string.Empty;
    public int? Floor { get; init; } = null;
    public string FullAddress { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public DateTimeOffset? UpdatedAt { get; init; } = null; 
    public bool IsDeleted { get; init; }
    public string ApplicationUserId { get; init; } = string.Empty;
    public int CountryId { get; init; }
    public int CityId { get; init; }
}