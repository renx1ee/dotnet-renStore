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
    private Country? _country { get; set; }
    public int CityId { get; private set; }
    private City? _city { get; set; }
    
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
        int floor,
        Country country,
        City city,
        Guid userId,
        DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(houseCode))
            throw new DomainException("House Code cannot be null or empty!");
        
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street cannot be null or empty!");
        
        if (string.IsNullOrWhiteSpace(buildingNumber))
            throw new DomainException("Building Number cannot be null or empty!");
        
        if(country == null)
            throw new DomainException("Country cannot be null.");
        
        if (country.Id <= 0)
            throw new DomainException("Country ID cannot be less 1.");
        
        if (country.IsDeleted)
            throw new DomainException("Country cannot be deleted.");
        
        if(city == null)
            throw new DomainException("City cannot be null.");
        
        if (city.Id <= 0)
            throw new DomainException("City ID cannot be  less 1.");
        
        if (city.IsDeleted)
            throw new DomainException("City cannot be deleted.");
        
        if (userId == Guid.Empty)
            throw new DomainException("User ID cannot be less 1.");
        
        var result = new Address()
        {
            Id = Guid.NewGuid(),
            HouseCode = houseCode.Trim(),
            Street = street.Trim(),
            BuildingNumber = buildingNumber.Trim(),
            Floor = floor,
            CreatedAt = now,
            IsDeleted = false,
            CountryId = country.Id,
            CityId = city.Id,
            ApplicationUserId = userId,
            _country = country,
            _city = city
        };

        if (!string.IsNullOrWhiteSpace(apartmentNumber))
            result.ApartmentNumber = apartmentNumber.Trim();
        
        if (!string.IsNullOrWhiteSpace(entrance))
            result.Entrance = entrance.Trim();

        result.BuildFullAddress();
        return result;
    }
    
    /// <summary>
    /// Updates an address data.
    /// Cannot be called with deleted address.
    /// </summary>
    /// <exception cref="DomainException">if the city is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public void Edit(
        DateTimeOffset now,
        string houseCode,
        string street,
        string buildingNumber,
        string? apartmentNumber = null,
        string? entrance = null,
        int? floor = null)
    {
        ValidateAddress(
            houseCode: houseCode, 
            street: street, 
            buildingNumber: buildingNumber);
        
        HouseCode = houseCode;
        Street = street;
        BuildingNumber = buildingNumber;
        Floor = floor;
        UpdatedAt = now;
        
        if(!string.IsNullOrWhiteSpace(apartmentNumber))
            ApartmentNumber = apartmentNumber.Trim();
        
        if(!string.IsNullOrWhiteSpace(entrance))
            Entrance = entrance.Trim();
        
        BuildFullAddress();
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
    
    private void BuildFullAddress()
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

    private void EnsureNotDeleted()
    {
        if (IsDeleted)
            throw new DomainException("Cannot update deleted address");
    }

    private void ValidateAddress(
        string houseCode,
        string street,
        string buildingNumber)
    {
        EnsureNotDeleted();
        
        if (string.IsNullOrWhiteSpace(houseCode))
            throw new DomainException("House Code cannot be null or empty!");
        
        if (string.IsNullOrWhiteSpace(street))
            throw new DomainException("Street cannot be null or empty!");
        
        if (string.IsNullOrWhiteSpace(buildingNumber))
            throw new DomainException("Building Number cannot be null or empty!");
    }
}