using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;
/// <summary>
/// Represents a user's physical address with lifecycle and invariants.
/// </summary>
public class Address
{
    public Guid Id { get; private set; }
    public string HouseCode { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string BuildingNumber  { get; private set; } = string.Empty;
    public string ApartmentNumber { get; private set; } = string.Empty;
    public string Entrance { get; private set; } = string.Empty;
    public int? Floor { get; private set; }
    public string FullAddress { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; } 
    public bool IsDeleted { get; private set; }
    public string ApplicationUserId { get; private set; } = string.Empty;
    public int CountryId { get; private set; }
    public Country? Country { get; private set; }
    public int CityId { get; private set; }
    public City? City { get; private set; }
    // public ICollection<DeliveryOrder>? Deliveries { get; set; } = new List<DeliveryOrder>();
    
    private Address() { }

    #region перенести в пользователя
    /// <summary>
    /// Creates a new address ensuring all invariants are satisfied.
    /// </summary>
    public static Address Create(
        Guid id,
        string houseCode,
        string street,
        string buildingNumber,
        string? apartmentNumber,
        string? entrance,
        int floor,
        int countryId,
        int cityId,
        string userId,
        DateTimeOffset now)
    {
        // TODO: validation
        return new Address()
        {
            Id = id,
            HouseCode = houseCode,
            Street = street,
            BuildingNumber = buildingNumber,
            ApartmentNumber = apartmentNumber,
            Entrance = entrance,
            Floor = floor,
            CountryId = countryId,
            CityId = cityId,
            ApplicationUserId = userId,
            CreatedAt = now,
            IsDeleted = false
        };
    }
    /// <summary>
    /// Updates an address data.
    /// Cannot be called with deleted address.
    /// </summary>
    public void Update(
        string houseCode,
        string street,
        string buildingNumber,
        string? apartmentNumber,
        string? entrance,
        int floor,
        DateTimeOffset now)
    {
        // TODO: validation
        HouseCode = houseCode;
        Street = street;
        BuildingNumber = buildingNumber;
        ApartmentNumber = apartmentNumber;
        Entrance = entrance;
        Floor = floor;
        UpdatedAt = now;
    }
    /// <summary>
    /// Soft delete the country.
    /// Once deleted the country cannot be modified.
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void Delete(DateTimeOffset now)
    {
        if (IsDeleted) 
            throw new DomainException("Address already deleted!");

        IsDeleted = true;
        UpdatedAt = now;
    }
    
    public void Validate()
    {
        
    }
    
    #endregion
}