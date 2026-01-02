using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Domain.Entities;
/// <summary>
/// Represents a country and its related cities and addresses.
/// </summary>
public class Country
{
    private readonly List<City> _cities = new();
    
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string NormalizedName { get; private set; } = string.Empty;
    public string OtherName { get; private set; } = string.Empty;
    public string NormalizedOtherName { get; private set; } = string.Empty;
    public string NameRu { get; private set; } = string.Empty;
    public string NormalizedNameRu { get; private set; } = string.Empty;
    /// <summary>
    /// ISO 3166-1 alpha-2 country code.
    /// </summary>
    public string Code { get; private set; } = string.Empty;
    public string PhoneCode { get; private set; } = string.Empty;
    public bool IsDeleted { get; private set; }
    public IEnumerable<Address>? Addresses { get; set; }
    public IReadOnlyCollection<City> Cities => _cities;
    /*public IEnumerable<ProductDetailEntity>? ProductDetails { get; set; }*/
    
    private Country(){ }
    /// <summary>
    /// Creates a new country ensuring all invariants are satisfied.
    /// </summary>
    public static Country Create(
        string name, 
        string otherName,
        string nameRu,
        string code,
        string phoneCode)
    {
        // TODO: validation
        return new Country()
        {
            Name = name,
            OtherName = otherName,
            NameRu = nameRu,
            Code = code,
            PhoneCode = phoneCode,
            IsDeleted = false
        };
    }
    /// <summary>
    /// Updates country data.
    /// Cannot be called with deleted country.
    /// </summary>
    public void Update(
        string name, 
        string otherName,
        string nameRu,
        string code,
        string phoneCode)
    {
        // TODO: validation
        Name = name;
        OtherName = otherName;
        NameRu = nameRu;
        Code = code;
        PhoneCode = phoneCode;
    }
    /// <summary>
    /// Soft delete the country.
    /// Once deleted the country cannot be modified.
    /// </summary>
    /// <exception cref="DomainException"></exception>
    public void Delete()
    {
        if (IsDeleted) 
            throw new DomainException("Country already deleted!");

        IsDeleted = true;
    }
}