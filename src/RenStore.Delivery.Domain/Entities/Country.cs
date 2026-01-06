using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;

/// <summary>
/// Represents a country and its related cities and addresses.
/// </summary>
public class Country
{
    private readonly List<Address> _addresses = new();
    private readonly List<City> _cities = new();
    
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public string NameRu { get; private set; } = string.Empty;
    public string NormalizedNameRu { get; private set; } = string.Empty;
    /// <summary>
    /// ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string Code { get; private set; } = string.Empty;
    public string PhoneCode { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }
    public IReadOnlyCollection<Address> Addresses => _addresses;
    public IReadOnlyCollection<City> Cities => _cities;
    
    private Country(){ }
    /// <summary>
    /// Creates a new country ensuring all invariants are satisfied.
    /// </summary>
    /// <exception cref="DomainException">if the country parameters are null or empty, or any IDs are less 0.</exception>
    public static Country Create(
        string name, 
        string nameRu,
        string code,
        string phoneCode)
    {
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be null or empty!");
        
        if (string.IsNullOrEmpty(nameRu))
            throw new DomainException("Name RU cannot be null or empty!");
        
        if (string.IsNullOrEmpty(code))
            throw new DomainException("Code cannot be null or empty!");
        
        if (string.IsNullOrEmpty(phoneCode))
            throw new DomainException("Phone Code cannot be null or empty!");
        
        return new Country()
        {
            Name = name,
            NormalizedName = name.ToUpperInvariant(),
            NameRu = nameRu,
            NormalizedNameRu = name.ToUpperInvariant(),
            Code = code,
            PhoneCode = phoneCode,
            IsDeleted = false
        };
    }
    /// <summary>
    /// Updates country data.
    /// Cannot be called with deleted country.
    /// </summary>
    /// <exception cref="DomainException">if the country is marked as deleted, or any of the input parameters are null or empty, or any IDs are less 0.</exception>
    public void Update(
        int countryId,
        string name, 
        string nameRu,
        string code,
        string phoneCode)
    {
        if (IsDeleted) 
            throw new DomainException("Cannot update deleted country!");
        
        if (countryId <= 0)
            throw new DomainException("CountryId cannot be less 1.");
        
        if (string.IsNullOrEmpty(name))
            throw new DomainException("Name cannot be null or empty!");
        
        if (string.IsNullOrEmpty(nameRu))
            throw new DomainException("Name RU cannot be null or empty!");
        
        if (string.IsNullOrEmpty(code))
            throw new DomainException("Code cannot be null or empty!");
        
        if (string.IsNullOrEmpty(phoneCode))
            throw new DomainException("Phone Code cannot be null or empty!");
        
        Name = name;
        NormalizedName = name.ToUpperInvariant();
        NameRu = nameRu;
        NormalizedNameRu = name.ToUpperInvariant();
        Code = code;
        PhoneCode = phoneCode;
    }
    /// <summary>
    /// Soft delete the country.
    /// Once deleted the country cannot be modified.
    /// </summary>
    /// <exception cref="DomainException">Throw if country already deleted.</exception>
    public void Delete()
    {
        if (IsDeleted) 
            throw new DomainException("Country already deleted!");

        IsDeleted = true;
    }
}