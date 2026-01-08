namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities.Address;

public class AddressTests
{
    private readonly Delivery.Domain.Entities.City _city;
    private readonly Delivery.Domain.Entities.Country _country;
    
    public AddressTests()
    {
        this._country = Delivery.Domain.Entities.Country.Create(
            name: "Russia",
            nameRu: "Russia",
            code: "ru",
            phoneCode: "+7",
            DateTimeOffset.UtcNow);

        this._city = Delivery.Domain.Entities.City.Create(
            name: "name",
            nameRu: "nameRu",
            countryId: 1,
            now: DateTimeOffset.UtcNow);
    }

    [Theory]
    [InlineData("re", "street", "build", "", "", null, 1, 1)]
    [InlineData("re", "street", "build", "wfw", "twet", 3, 1, 1)]
    public async Task CreateAddress_Success_Test(
        string houseCode,
        string street,
        string buildingNumber,
        string? apartmentNumber,
        string? entrance,
        int? floor,
        int countryId,
        int cityId)
    {
        // Arrange
        var userId = Guid.NewGuid();
        // Act
        var result = Delivery.Domain.Entities.Address.Create(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            countryId: countryId,
            cityId: cityId,
            userId: userId,
            DateTimeOffset.UtcNow);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(houseCode, result.HouseCode);
        Assert.Equal(street, result.Street);
        Assert.Equal(buildingNumber, result.BuildingNumber);
        Assert.Equal(apartmentNumber, result.ApartmentNumber);
        Assert.Equal(entrance, result.Entrance);
        Assert.Equal(1, result.Floor);
        Assert.Equal(countryId, result.CountryId);
        Assert.Equal(cityId, result.CountryId);
        Assert.Equal(userId, result.ApplicationUserId);
    }
}