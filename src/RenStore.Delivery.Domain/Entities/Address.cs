using RenStore.Delivery.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Represents a user's physical address with lifecycle and invariants.
/// </summary>
public class Address
{
    private FullMultiplyAddress _fullAddress;
    
    public Guid Id { get; private set; }
    public string HouseCode { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string BuildingNumber { get; private set; } = string.Empty;
    public string ApartmentNumber { get; private set; } = string.Empty;
    public string Entrance { get; private set; } = string.Empty;
    public int? Floor { get; private set; }
    public string FullAddressEn => _fullAddress.English; 
    public string FullAddressRu => _fullAddress.Russian;  
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; } 
    public DateTimeOffset? DeletedAt { get; private set; } 
    public bool IsDeleted { get; private set; }
    public Guid ApplicationUserId { get; private set; }
    public int CountryId { get; private set; }
    private Country? _country { get; }
    public int CityId { get; private set; }
    private City? _city { get; }
    
    private Address() { }
    
    /// <summary>
    /// Creates a new address ensuring all invariants are satisfied.
    /// </summary>
    /// <exception cref="DomainException">if any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public static Address Create(
        string houseCode,
        string street,
        string buildingNumber,
        string? apartmentNumber,
        string? entrance,
        int? floor,
        int countryId,
        int cityId,
        Guid userId,
        DateTimeOffset now)
    {
        if (string.IsNullOrEmpty(houseCode))
            throw new DomainException("House Code cannot be null or empty!");
        
        if (string.IsNullOrEmpty(street))
            throw new DomainException("Street cannot be null or empty!");
        
        if (string.IsNullOrEmpty(buildingNumber))
            throw new DomainException("Building Number cannot be null or empty!");
        
        if (countryId <= 0)
            throw new DomainException("Country ID cannot be less 1.");
        
        if (cityId <= 0)
            throw new DomainException("Country ID cannot be  less 1.");
        
        if (userId == Guid.Empty)
            throw new DomainException("User ID cannot be less 1.");
        
        var result =  new Address()
        {
            Id = Guid.NewGuid(),
            HouseCode = houseCode,
            Street = street,
            BuildingNumber = buildingNumber,
            Floor = floor ?? 1,
            CountryId = countryId,
            CityId = cityId,
            ApplicationUserId = userId,
            CreatedAt = now,
            IsDeleted = false
        };

        if (!string.IsNullOrEmpty(apartmentNumber))
            result.ApartmentNumber = apartmentNumber;
        
        if (!string.IsNullOrEmpty(entrance))
            result.Entrance = entrance;

        

        return result;
    }
    
    /// <summary>
    /// Updates an address data.
    /// Cannot be called with deleted address.
    /// </summary>
    /// <exception cref="DomainException">if the city is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public void Update(
        string houseCode,
        string street,
        string buildingNumber,
        string? apartmentNumber,
        string? entrance,
        int floor,
        DateTimeOffset now)
    {
        if (IsDeleted)
            throw new DomainException("Cannot update deleted address");
        
        HouseCode = houseCode;
        Street = street;
        BuildingNumber = buildingNumber;
        Floor = floor;
        UpdatedAt = now;
        
        if(!string.IsNullOrEmpty(apartmentNumber))
            ApartmentNumber = apartmentNumber;
        
        if(!string.IsNullOrEmpty(entrance))
            Entrance = entrance;
        
        UpdateFullAddress();
    }
    
    /// <summary>
    /// Soft delete the country.
    /// Once deleted the country cannot be modified.
    /// </summary>
    /// <exception cref="DomainException">Throw if address already marked as deleted.</exception>
    public void Delete(DateTimeOffset now)
    {
        if (IsDeleted) 
            throw new DomainException("Address already deleted!");

        IsDeleted = true;
        UpdatedAt = now;
    }

    private void UpdateFullAddress()
    {
        _fullAddress = FullMultiplyAddress.BuildFull(
            country: _country,
            city: _city,
            street: Street,
            buildingNumber: BuildingNumber,
            houseCode: HouseCode,
            apartmentNumber: ApartmentNumber,
            entrance: Entrance,
            floor: Floor);
    }
}