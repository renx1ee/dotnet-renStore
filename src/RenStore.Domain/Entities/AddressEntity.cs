namespace RenStore.Domain.Entities;

public class AddressEntity
{
    public Guid Id { get; set; }
    public string HouseCode { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string BuildingNumber  { get; set; } = string.Empty;
    public string ApartmentNumber { get; set; } = string.Empty;
    public string Entrance { get; set; } = string.Empty;
    public uint Floor { get; set; }
    public string FlatNumber { get; set; } = string.Empty;
    public string FullAddress { get; set; } = string.Empty; // TODO: сделать функцию в бд для создания полного аддреса
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public string ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public int CountryId { get; set; }
    public CountryEntity? Country { get; set; }
    public int CityId { get; set; }
    public CityEntity? City { get; set; }
    public IEnumerable<DeliveryOrderEntity>? Deliveries { get; set; }
}