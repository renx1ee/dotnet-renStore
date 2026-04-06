using System.Globalization;
using RenStore.Catalog.Domain.Constants;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Domain.ValueObjects;

public sealed class CountryOfManufacture
{
    public string Name { get; }

    public CountryOfManufacture(string countryName)
    {
        var normalizedName = NormalizeName(countryName);
        NameValidation(normalizedName);

        Name = normalizedName;
    }
    
    public static void NameValidation(string countryName)
    {
        if (string.IsNullOrWhiteSpace(countryName))
            throw new DomainException(
                "Country name cannot be null or white space.");

        if (countryName.Length is > CatalogConstants.ProductDetail.MaxCountryOfManufactureLength
                               or < CatalogConstants.ProductDetail.MinCountryOfManufactureLength)
        {
            throw new DomainException(
                "Country of manufacture name lenght must be between " +
                $"{CatalogConstants.ProductDetail.MaxCountryOfManufactureLength} and " +
                $"{CatalogConstants.ProductDetail.MinCountryOfManufactureLength} characters.");
        }
    }

    private static string NormalizeName(string name)
    {
        var trimmedName = name.Trim().ToLower();
        
        return char.ToUpper(trimmedName[0], CultureInfo.CurrentCulture) + trimmedName.Substring(1);
    }
}