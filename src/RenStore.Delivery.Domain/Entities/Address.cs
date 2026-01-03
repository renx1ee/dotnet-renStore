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
    
    private Address() { }

    #region перенести в пользователя
    /// <summary>
    /// Creates a new address ensuring all invariants are satisfied.
    /// </summary>
    /// <exception cref="DomainException">if the city is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
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
        
        if (string.IsNullOrEmpty(userId))
            throw new DomainException("User ID cannot be less 1.");
        
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
        ApartmentNumber = apartmentNumber;
        Entrance = entrance;
        Floor = floor;
        UpdatedAt = now;
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
    #endregion
}