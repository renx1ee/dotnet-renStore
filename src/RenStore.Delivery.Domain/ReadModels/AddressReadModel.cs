namespace RenStore.Delivery.Domain.ReadModels;

public sealed class AddressReadModel
{
    public Guid            Id                { get; set; }
    public Guid            ApplicationUserId { get; set; }
    public int             CountryId         { get; set; }
    public int             CityId            { get; set; }
    public string          Street            { get; set; } = string.Empty;
    public string          HouseCode         { get; set; } = string.Empty;
    public string          BuildingNumber    { get; set; } = string.Empty;
    public string          ApartmentNumber   { get; set; } = string.Empty;
    public string          Entrance          { get; set; } = string.Empty;
    public int?            Floor             { get; set; }
    public string          FullAddressEn     { get; set; } = string.Empty;
    public string          FullAddressRu     { get; set; } = string.Empty;
    public string          Postcode          { get; set; } = string.Empty;
    public bool            IsDeleted         { get; set; }
    public DateTimeOffset  CreatedAt         { get; set; }
    public DateTimeOffset? UpdatedAt         { get; set; }
    public DateTimeOffset? DeletedAt         { get; set; }
}