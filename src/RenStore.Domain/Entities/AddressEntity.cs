namespace RenStore.Domain.Entities;
/// <summary>
/// 
/// </summary>
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
    public string FullAddress { get; set; } = string.Empty;
    public string ApplicationUserId { get; set; }
    public ApplicationUser? ApplicationUser { get; set; }
    public int CountryId { get; set; }
    public CountryEntity? Country { get; set; }
    public int CityId { get; set; }
    public CityEntity? City { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    /*public IList<Order> Orders { get; set; } */
    /*public Guid DeliveryId { get; set; }
    public Delivery? Delivery { get; set;}*/
}