using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Represents a city physical entity with lifecycle and invariants. 
/// </summary>
public class City
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NameRu { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public string NormalizedNameRu { get; private set; } = string.Empty;
    public bool IsDelete { get; private set; } = false;
    public int CountryId { get; private set; }
    public Country? Country { get; private set; }
    // TODO:
    public IReadOnlyCollection<Address>? Addresses { get; private set; }
    
    private City() { }
    /// <summary>
    /// Creates a new city ensuring all invariants are satisfied.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DomainException">if the city is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public static City Create(
        string name,
        string nameRu,
        int countryId)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be null or empty!");
        
        if (string.IsNullOrEmpty(nameRu))
            throw new DomainException("Name RU cannot be null or empty!");
        
        if (countryId <= 0)
            throw new DomainException("CountryId cannot be less 1.");
        
        return new City()
        {
            Name = name,
            NormalizedName = name.ToUpperInvariant(),
            NameRu = nameRu,
            NormalizedNameRu = nameRu.ToUpperInvariant(),
            CountryId = countryId
        };
    }
    /// <summary>
    /// Updates a city data.
    /// Cannot be called with deleted city.
    /// </summary>
    /// <exception cref="DomainException">if the city is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public void Update(
        int cityId,
        string name,
        string nameRu)
    {
        if (IsDelete)
            throw new DomainException("Cannot update already deleted city.");
        
        if (cityId <= 0)
            throw new DomainException("CountryId cannot be less 1.");
        
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be null or empty!");
        
        if (string.IsNullOrEmpty(nameRu))
            throw new DomainException("Name RU cannot be null or empty!");
        
        Name = name;
        NameRu = nameRu;
    }
    /// <summary>
    /// Soft delete the city.
    /// Once deleted the city cannot be modified.
    /// </summary>
    /// <exception cref="DomainException">Throw if city already marked as deleted.</exception>
    public void Delete()
    {
        if (IsDelete)
            throw new DomainException("Cannot delete already deleted city.");

        IsDelete = true;
    }
}