using System.Text;
using RenStore.Delivery.Domain.Entities;

namespace RenStore.Delivery.Domain.ValueObjects;

public sealed class FullMultiplyAddress
{
    public string Russian { get; private set; }
    public string English { get; private set; }
    
    private FullMultiplyAddress(string russian, string english)
    {
        Russian = russian ?? throw new ArgumentNullException(nameof(russian));
        English = english ?? throw new ArgumentNullException(nameof(english));
    }

    public static FullMultiplyAddress BuildFull(
        Country country,
        City city,
        string street, 
        string buildingNumber, 
        string? houseCode, 
        string? apartmentNumber, 
        string? entrance, 
        int? floor)
    {
        if (country == null) 
            throw new ArgumentNullException(nameof(country));
        
        if (city == null) 
            throw new ArgumentNullException(nameof(city));

        if (string.IsNullOrWhiteSpace(street))
            throw new ArgumentNullException(nameof(street)); 
        
        if (string.IsNullOrWhiteSpace(buildingNumber))
            throw new ArgumentNullException(nameof(buildingNumber));
        
        var addressRu = BuildFullRussianAddress(
            country: country,
            city: city,
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor);
        
        var addressEn = BuildFullEnglishAddress(
            country: country,
            city: city,
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor);

        return new FullMultiplyAddress(
            russian: addressRu, 
            english: addressEn);
    }

    private static string BuildFullEnglishAddress(
        Country country,
        City city,
        string street, 
        string buildingNumber, 
        string? houseCode, 
        string? apartmentNumber, 
        string? entrance, 
        int? floor)
    {
        var result = new StringBuilder(string.Empty);

        if (string.IsNullOrWhiteSpace(country.Name))
            throw new ArgumentNullException(nameof(country.Name));
    
        result.Append("Country: " + country.Name);
        
        if (string.IsNullOrWhiteSpace(city.Name))
            throw new ArgumentNullException(nameof(city.Name));
        
        result.Append(", City: " + city.Name);

        if (!string.IsNullOrWhiteSpace(houseCode))
            result.Append(", House code: " + houseCode);
    
        if (!string.IsNullOrWhiteSpace(street))
            result.Append(", Street: " + street);
    
        if (!string.IsNullOrWhiteSpace(buildingNumber))
            result.Append(", Building number: " + buildingNumber);
    
        if (!string.IsNullOrWhiteSpace(apartmentNumber))
            result.Append(", Apartment number: " + apartmentNumber);
    
        if (!string.IsNullOrWhiteSpace(entrance))
            result.Append(", Entrance: " + entrance);

        result.Append(", Floor: " + floor.ToString());
        
        return result.ToString();
    }

    private static string BuildFullRussianAddress(
        Country country,
        City city,
        string street,
        string buildingNumber,
        string? houseCode,
        string? apartmentNumber,
        string? entrance,
        int? floor)
    {
        var result = new StringBuilder(string.Empty);
        
        if (string.IsNullOrEmpty(country.NameRu))
            throw new ArgumentNullException(nameof(country.NameRu));
        
        result.Append("Страна: " + country.NameRu);
            
        if (string.IsNullOrEmpty(city.NameRu))
            throw new ArgumentNullException(nameof(city.NameRu));
            
        result.Append(", Город: " + city.NameRu);

        if (!string.IsNullOrEmpty(houseCode))
            result.Append(", Корпус: " + houseCode);
        
        if (!string.IsNullOrEmpty(street))
            result.Append(", Улица: " + street);
        
        if (!string.IsNullOrEmpty(buildingNumber))
            result.Append(", Номер дома: " + buildingNumber);
        
        if (!string.IsNullOrEmpty(apartmentNumber))
            result.Append(", Номер квартиры: " + apartmentNumber);
        
        if (!string.IsNullOrEmpty(entrance))
            result.Append(", Вход: " + entrance);

        result.Append(", Этаж: " + floor.ToString());
        
        return result.ToString();
    }
}
