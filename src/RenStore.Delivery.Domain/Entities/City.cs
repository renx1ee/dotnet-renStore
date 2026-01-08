using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Represents a city physical entity with lifecycle and invariants. 
/// </summary>
public class City
{
    private readonly List<Address> _addresses = new();
    
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NameRu { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public string NormalizedNameRu { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; } = false;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }
    public int CountryId { get; private set; }
    private Country? _country { get; }

    public IReadOnlyCollection<Address>? Addresses => _addresses.AsReadOnly();
    
    private City() { }
    
    /// <summary>
    /// Creates a new city ensuring all invariants are satisfied.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="DomainException">if the city is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public static City Create(
        string name,
        string nameRu,
        int countryId,
        DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be null or empty!");
        
        if (string.IsNullOrWhiteSpace(nameRu))
            throw new DomainException("Name RU cannot be null or empty!");
        
        if (countryId <= 0)
            throw new DomainException("CountryId cannot be less 1.");
        
        return new City()
        {
            Name = name.Trim(),
            NormalizedName = name.Trim().ToUpperInvariant(),
            NameRu = nameRu.Trim(),
            NormalizedNameRu = nameRu.Trim().ToUpperInvariant(),
            CountryId = countryId,
            CreatedAt = now,
            IsDeleted = false
        };
    }
    
    /// <summary>
    /// Updates a city data.
    /// Cannot be called with deleted city.
    /// </summary>
    /// <exception cref="DomainException">if the city is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public void Update(
        string name,
        string nameRu,
        DateTimeOffset now)
    {
        if (IsDeleted)
            throw new DomainException("Cannot update already deleted city.");
        
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Name cannot be null or empty!");
        
        if (string.IsNullOrWhiteSpace(nameRu))
            throw new DomainException("Name RU cannot be null or empty!");
        
        Name = name.Trim();
        NormalizedName = name.Trim().ToUpperInvariant();
        NameRu = nameRu.Trim();
        NormalizedNameRu = nameRu.Trim().ToUpperInvariant();
        UpdatedAt = now;
    }
    
    /// <summary>
    /// Soft delete the city.
    /// Once deleted the city cannot be modified.
    /// </summary>
    /// <exception cref="DomainException">Throw if city already marked as deleted.</exception>
    public void Delete(DateTimeOffset now)
    {
        if (IsDeleted)
            throw new DomainException("Cannot delete already deleted city.");

        IsDeleted = true;
        DeletedAt = now;
        UpdatedAt = now;
    }
}