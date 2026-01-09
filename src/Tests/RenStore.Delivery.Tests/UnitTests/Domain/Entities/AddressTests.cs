using RenStore.Delivery.Tests.Shared;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities;

public sealed class AddressTests : IClassFixture<TestDataFactory>
{
    private readonly TestDataFactory _dataFactory;
    
    public AddressTests(TestDataFactory dataDataFactory)
    {
        this._dataFactory = dataDataFactory;
    }

    [Theory]
    [InlineData("re", "street", "build", "", "", 1, 1, 1)]
    [InlineData("re", "street", "build", "wfw", "twet", 3, 1, 1)]
    public async Task CreateAddress_Success_Test(
        string houseCode,
        string street,
        string buildingNumber,
        string? apartmentNumber,
        string? entrance,
        int floor,
        int countryId,
        int cityId)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var context = DatabaseFixture.CreateReadyContext();
        var country = await this._dataFactory.CreateCountryAsync(context);
        var city = await this._dataFactory.CreateCityAsync(
            context: context, 
            country: country);
        
        // Act
        var result = Delivery.Domain.Entities.Address.Create(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            country: country,
            city: city,
            userId: userId,
            DateTimeOffset.UtcNow);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(houseCode, result.HouseCode);
        Assert.Equal(street, result.Street);
        Assert.Equal(buildingNumber, result.BuildingNumber);
        Assert.Equal(apartmentNumber, result.ApartmentNumber);
        Assert.Equal(entrance, result.Entrance);
        Assert.Equal(floor, result.Floor);
        Assert.Equal(countryId, result.CountryId);
        Assert.Equal(cityId, result.CountryId);
        Assert.Equal(userId, result.ApplicationUserId);
    }
    
    [Theory]
    [InlineData(null, "street", "build", 1)]
    [InlineData("", "street", "build", 1)]
    [InlineData(" ", "street", "build", 1)]
    [InlineData("re", null, "build", 1)]
    [InlineData("re", "", "build", 3)]
    [InlineData("re", " ", "build", 2)]
    [InlineData("re", "street", null, 1)]
    [InlineData("re", "street", "", 1)]
    [InlineData("re", "street", " ", 1)]
    public async Task CreateAddress_FailOnParameters_Test(
        string houseCode,
        string street,
        string buildingNumber,
        int floor)
    {
        // Arrange
        var userId = Guid.NewGuid();
        var context = DatabaseFixture.CreateReadyContext();
        var country = await this._dataFactory.CreateCountryAsync(context);
        var city = await this._dataFactory.CreateCityAsync(
            context: context, 
            country: country);

        // Act & Assert
        Assert.Throws<DomainException>(() => 
            Delivery.Domain.Entities.Address.Create(
                houseCode: houseCode,
                street: street,
                buildingNumber: buildingNumber,
                apartmentNumber: null,
                entrance: null,
                floor: 1,
                country: country,
                city: city,
                userId: userId,
                DateTimeOffset.UtcNow));

    }
    
    [Theory]
    [InlineData("re", "street", "build", "wfw", "twet", 3)]
    public async Task EditAddress_Success_Test(
        string newHouseCode,
        string newStreet,
        string newBuildingNumber,
        string newApartmentNumber,
        string newEntrance,
        int? newFloor)
    {
        // Arrange
        var userId = Guid.NewGuid();
        string houseCode = "Hs";
        string street = "street";
        string buildingNumber = "43";
        string apartmentNumber = "df";
        string entrance = "ww";
        int floor = 6;
        
        var context = DatabaseFixture.CreateReadyContext();
        var country = await this._dataFactory.CreateCountryAsync(context);
        var city = await this._dataFactory.CreateCityAsync(
            context: context, 
            country: country);
        
        // Act
        var result = Delivery.Domain.Entities.Address.Create(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            country: country,
            city: city,
            userId: userId,
            DateTimeOffset.UtcNow);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(houseCode, result.HouseCode);
        Assert.Equal(street, result.Street);
        Assert.Equal(buildingNumber, result.BuildingNumber);
        Assert.Equal(apartmentNumber, result.ApartmentNumber);
        Assert.Equal(entrance, result.Entrance);
        Assert.Equal(floor, result.Floor);
        Assert.Equal(country.Id, result.CountryId);
        Assert.Equal(city.Id, result.CountryId);
        Assert.Equal(userId, result.ApplicationUserId);
        
        result.Edit(
            houseCode: newHouseCode,
            street: newStreet,
            buildingNumber: newBuildingNumber,
            apartmentNumber: newApartmentNumber,
            entrance: newEntrance,
            floor: newFloor,
            now: DateTimeOffset.UtcNow);
        
        Assert.NotNull(result);
        Assert.Equal(newHouseCode, result.HouseCode);
        Assert.Equal(newStreet, result.Street);
        Assert.Equal(newBuildingNumber, result.BuildingNumber);
        Assert.Equal(newApartmentNumber, result.ApartmentNumber);
        Assert.Equal(newEntrance, result.Entrance);
        Assert.Equal(newFloor, result.Floor);
        Assert.Equal(country.Id, result.CountryId);
        Assert.Equal(city.Id, result.CountryId);
        Assert.Equal(userId, result.ApplicationUserId);
    }
    
    [Theory]
    [InlineData(null, "street", "build", "wfw")]
    [InlineData("", "street", "build", "wfw")]
    [InlineData(" ", "street", "build", "wfw")]
    [InlineData("re", null, "build", "wfw")]
    [InlineData("re", "", "build", "wfw")]
    [InlineData("re", " ", "build", "wfw")]
    [InlineData("re", "street", null, "wfw")]
    [InlineData("re", "street", "", "wfw")]
    [InlineData("re", "street", " ", null)]
    [InlineData("re", "street", " ", "")]
    [InlineData("re", "street", " ", " ")]
    public async Task EditAddress_FailOnWrongParameters_Test(
        string newHouseCode,
        string newStreet,
        string newBuildingNumber,
        string newApartmentNumber)
    {
        // Arrange
        var userId = Guid.NewGuid();
        string houseCode = "Hs";
        string street = "street";
        string buildingNumber = "43";
        string apartmentNumber = "df";
        string entrance = "ww";
        int floor = 6;
        
        var context = DatabaseFixture.CreateReadyContext();
        var country = await this._dataFactory.CreateCountryAsync(context);
        var city = await this._dataFactory.CreateCityAsync(
            context: context, 
            country: country);
        
        // Act
        var result = Delivery.Domain.Entities.Address.Create(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            country: country,
            city: city,
            userId: userId,
            DateTimeOffset.UtcNow);
        
        Assert.NotNull(result);
        
        // Assert
        Assert.Throws<DomainException>(() => result.Edit(
            houseCode: newHouseCode,
            street: newStreet,
            buildingNumber: newBuildingNumber,
            apartmentNumber: newApartmentNumber,
            entrance: entrance,
            floor: floor,
            now: DateTimeOffset.UtcNow));
    }

    [Fact]
    public async Task EditAddress_FailOnDeleted_Test()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string houseCode = "Hs";
        string street = "street";
        string buildingNumber = "43";
        string apartmentNumber = "df";
        string entrance = "ww";
        int floor = 6;
        var now = DateTimeOffset.UtcNow;
        
        var context = DatabaseFixture.CreateReadyContext();
        var country = await this._dataFactory.CreateCountryAsync(context);
        var city = await this._dataFactory.CreateCityAsync(
            context: context, 
            country: country);
        
        // Act
        var result = Delivery.Domain.Entities.Address.Create(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            country: country,
            city: city,
            userId: userId,
            now: now);
        
        Assert.NotNull(result);
        
        result.Delete(now: now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.Edit(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            now: now));
    }
    
    [Fact]
    public async Task DeleteAddress_Success_Test()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string houseCode = "Hs";
        string street = "street";
        string buildingNumber = "43";
        string apartmentNumber = "df";
        string entrance = "ww";
        int floor = 6;
        var now = DateTimeOffset.UtcNow;
        
        var context = DatabaseFixture.CreateReadyContext();
        var country = await this._dataFactory.CreateCountryAsync(context);
        var city = await this._dataFactory.CreateCityAsync(
            context: context, 
            country: country);
        
        // Act
        var result = Delivery.Domain.Entities.Address.Create(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            country: country,
            city: city,
            userId: userId,
            now: now);
        
        Assert.NotNull(result);
        
        result.Delete(now: now);
        
        // Assert
        Assert.True(result.IsDeleted);
    }
    
    [Fact]
    public async Task DeleteAddress_FailOnDeleted_Test()
    {
        // Arrange
        var userId = Guid.NewGuid();
        string houseCode = "Hs";
        string street = "street";
        string buildingNumber = "43";
        string apartmentNumber = "df";
        string entrance = "ww";
        int floor = 6;
        var now = DateTimeOffset.UtcNow;
        
        var context = DatabaseFixture.CreateReadyContext();
        var country = await this._dataFactory.CreateCountryAsync(context);
        var city = await this._dataFactory.CreateCityAsync(
            context: context, 
            country: country);
        
        // Act
        var result = Delivery.Domain.Entities.Address.Create(
            houseCode: houseCode,
            street: street,
            buildingNumber: buildingNumber,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor,
            country: country,
            city: city,
            userId: userId,
            now: now);
        
        Assert.NotNull(result);
        
        result.Delete(now: now);
        
        // Assert
        Assert.Throws<DomainException>(() =>  result.Delete(now: now));
    }
}