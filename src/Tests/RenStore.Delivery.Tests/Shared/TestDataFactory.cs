using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Persistence;

namespace RenStore.Delivery.Tests.Shared;

public class TestDataFactory
{
    public async Task<Country> CreateCountryAsync(
        DeliveryDbContext context,
        string? name = "Russia",
        string? nameRu = "Россия",
        string? code = "ru",
        string? phoneCode = "+7")
    {
        var country = Country.Create(
            name: name,
            nameRu: nameRu,
            code: code,
            phoneCode: phoneCode,
            now: DateTimeOffset.UtcNow);

        await context.Countries.AddAsync(country);
        await context.SaveChangesAsync();

        return country;
    }
    
    public async Task<City> CreateCityAsync(
        DeliveryDbContext context,
        Country country,
        string? name = "Moscow",
        string? nameRu = "Москва")
    {
        var city = City.Create(
            name: name,
            nameRu: nameRu,
            countryId: country.Id,
            now: DateTimeOffset.UtcNow);

        await context.Cities.AddAsync(city);
        await context.SaveChangesAsync();

        return city;
    }
}