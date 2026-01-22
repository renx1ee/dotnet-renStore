using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities;

public sealed class CityTests
{
    private readonly Delivery.Domain.Entities.Country _coutnry;

    public CityTests()
    {
        var result = _coutnry = Delivery.Domain.Entities.Country.Create(
            name: "Key", 
            nameRu: "NameRu", 
            code: "ru", 
            phoneCode:"+7",
            DateTimeOffset.UtcNow);
    }
    
    [Fact]
    public async Task CreateCity_Success_Test()
    {
        // Arrange
        string name = "City";
        string nameRu = "City";
        int countryId = 1;
        
        // Act
        var city = Delivery.Domain.Entities.City.Create(
            name: name, 
            nameRu: nameRu, 
            countryId: countryId,
            DateTimeOffset.UtcNow);
        
        // Assert
        Assert.NotNull(city);
        Assert.Equal(name, city.Name);
        Assert.Equal(name.ToUpperInvariant(), city.NormalizedName);
        Assert.Equal(nameRu, city.NameRu);
        Assert.Equal(nameRu.ToUpperInvariant(), city.NormalizedNameRu);
        Assert.Equal(countryId, city.CountryId);
    }
    
    [Theory]
    [InlineData(null, "Сити", 1)]
    [InlineData("", "Сити", 1)]
    [InlineData(" ", "Сити", 1)]
    [InlineData("City", null, 1)]
    [InlineData("City", "", 1)]
    [InlineData("City", " ", 1)]
    [InlineData("City", "Сити", 0)]
    [InlineData("City", "Сити", -1)]
    public async Task CreateCity_FailOnWrongParameters_Test(
        string name,
        string nameRu,
        int countryId)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            Delivery.Domain.Entities.City.Create(
                name: name,
                nameRu: nameRu,
                countryId: countryId,
                DateTimeOffset.UtcNow));
    }
    
    [Fact]
    public async Task UpdateCity_Success_Test()
    {
        // Arrange
        string name      = "City";
        string nameRu    = "City";
        int countryId    = 1;

        string newName   = "New Key";
        string newNameRu = "Новое имя";
        
        // Act
        var city = Delivery.Domain.Entities.City.Create(
            name: name, 
            nameRu: nameRu, 
            countryId: countryId,
            DateTimeOffset.UtcNow);
        
        Assert.NotNull(city);
        Assert.Equal(name, city.Name);
        Assert.Equal(name.ToUpperInvariant(), city.NormalizedName);
        Assert.Equal(nameRu, city.NameRu);
        Assert.Equal(nameRu.ToUpperInvariant(), city.NormalizedNameRu);
        Assert.Equal(countryId, city.CountryId);
        
        // Assert
        city.Update(
            name: newName, 
            nameRu: newNameRu,
            DateTimeOffset.UtcNow);
        
        Assert.NotNull(city);
        Assert.Equal(newName, city.Name);
        Assert.Equal(newName.ToUpperInvariant(), city.NormalizedName);
        Assert.Equal(newNameRu, city.NameRu);
        Assert.Equal(newNameRu.ToUpperInvariant(), city.NormalizedNameRu);
        Assert.Equal(countryId, city.CountryId);
        
    }
    
    [Theory]
    [InlineData(null, "Сити")]
    [InlineData("", "Сити")]
    [InlineData(" ", "Сити")]
    [InlineData("City", null)]
    [InlineData("City", "")]
    [InlineData("City", " ")]
    public async Task UpdateCity_FailOnWrongParameters_Test(
        string newName,
        string newNameRu)
    {
        // Arrange
        string name      = "City";
        string nameRu    = "City";
        int countryId    = 1;

        var now = DateTimeOffset.UtcNow;
        
        // Act
        var city = Delivery.Domain.Entities.City.Create(
            name: name, 
            nameRu: nameRu, 
            countryId: countryId,
            now: now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            city.Update(
                name: newName,
                nameRu: newNameRu,
                now: now));
    }
    
    [Fact]
    public async Task UpdateCity_FailOnDeletedEntity_Test()
    {
        // Arrange
        string name      = "City";
        string nameRu    = "City";
        int countryId    = 1;

        var now = DateTimeOffset.UtcNow;
        
        // Act
        var city = Delivery.Domain.Entities.City.Create(
            name: name, 
            nameRu: nameRu, 
            countryId: countryId,
            now: now);
        
        city.Delete(now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() =>
            city.Update(
                name: name,
                nameRu: nameRu,
                now: now));
    }
    
    [Fact]
    public async Task DeleteCity_Success_Test()
    {
        // Arrange
        string name      = "City";
        string nameRu    = "City";
        int countryId    = 1;

        var now = DateTimeOffset.UtcNow;
        
        // Act
        var city = Delivery.Domain.Entities.City.Create(
            name: name, 
            nameRu: nameRu, 
            countryId: countryId,
            now: now);
        
        city.Delete(now);
        
        // Assert
        Assert.True(city.IsDeleted);
    }
    
    [Fact]
    public async Task DeleteCity_FailOnAlreadyDeleted_Test()
    {
        // Arrange
        string name      = "City";
        string nameRu    = "City";
        int countryId    = 1;

        var now = DateTimeOffset.UtcNow;
        
        // Act
        var city = Delivery.Domain.Entities.City.Create(
            name: name, 
            nameRu: nameRu, 
            countryId: countryId,
            now: now);
        
        city.Delete(now);
        
        // Assert
        Assert.True(city.IsDeleted);

        Assert.Throws<DomainException>(() => city.Delete(now));
    }
}