using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities;

public sealed class CountryTests
{
    [Fact]
    public async Task CreateCountry_Success_Test()
    {
        // Arrange
        string name      = "Russia";
        string nameRu    = "Россия";
        string code      = "ru";
        string phoneCode = "+7";
        
        // Act
        var result = Delivery.Domain.Entities.Country.Create(
            name: name,
            nameRu: nameRu,
            code: code,
            phoneCode: phoneCode,
            DateTimeOffset.UtcNow);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(name.ToUpperInvariant(), result.NormalizedName);
        Assert.Equal(nameRu, result.NameRu);
        Assert.Equal(nameRu.ToUpperInvariant(), result.NormalizedNameRu);
        Assert.Equal(code, result.Code);
        Assert.Equal(phoneCode, result.PhoneCode);
    }
    
    [Theory]
    [InlineData(null, "Россия", "ru", "+7")]
    [InlineData("", "Россия", "ru", "+7")]
    [InlineData(" ", "Россия", "ru", "+7")]
    [InlineData("Russia", null, "ru", "+7")]
    [InlineData("Russia", "", "ru", "+7")]
    [InlineData("Russia", " ", "ru", "+7")]
    [InlineData("Russia", "Россия", null, "+7")]
    [InlineData("Russia", "Россия", "", "+7")]
    [InlineData("Russia", "Россия", " ", "+7")]
    [InlineData("Russia", "Россия", "ru", null)]
    [InlineData("Russia", "Россия", "ru", "")]
    [InlineData("Russia", "Россия", "ru", " ")]
    public async Task CreateCountry_Fail_Test(
        string name, 
        string nameRu,
        string code,
        string phoneCode)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            Delivery.Domain.Entities.Country.Create(
                name: name,
                nameRu: nameRu,
                code: code,
                phoneCode: phoneCode,
                DateTimeOffset.UtcNow));
    }
    
    [Theory]
    [InlineData("r", "+7")]
    [InlineData("rus", "+7")]
    [InlineData("ru", "+788")]
    public async Task CreateCountry_FailOnCodeSymbols_Test(string code, string phoneCode)
    {
        // Arrange
        string name = "Russia";
        string nameRu = "Россия";
        
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            Delivery.Domain.Entities.Country.Create(
                name: name,
                nameRu: nameRu,
                code: code,
                phoneCode: phoneCode,
                DateTimeOffset.UtcNow));
    }

    [Theory]
    [InlineData("Russia", "Россия", "ru", "+7")]
    public async Task UpdateCountry_Success_Test(
        string name, 
        string nameRu,
        string code,
        string phoneCode)
    {
        // Arrange
        string existingName      = "USA";
        string existingNameRu    = "Соединенные Штаты Америки";
        string existingCode      = "en";
        string existingPhoneCode = "+7";
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = Delivery.Domain.Entities.Country.Create(
            name: existingName,
            nameRu: existingNameRu,
            code: existingCode,
            phoneCode: existingPhoneCode,
            now);
        
        result.Update(
            name: name,
            nameRu: nameRu,
            code: code,
            phoneCode: phoneCode,
            now: now);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(name, result.Name);
        Assert.Equal(name.ToUpperInvariant(), result.NormalizedName);
        Assert.Equal(nameRu, result.NameRu);
        Assert.Equal(nameRu.ToUpperInvariant(), result.NormalizedNameRu);
        Assert.Equal(code, result.Code);
        Assert.Equal(phoneCode, result.PhoneCode);
    }
    
    [Theory]
    [InlineData(null, "Россия", "ru", "+7")]
    [InlineData("", "Россия", "ru", "+7")]
    [InlineData(" ", "Россия", "ru", "+7")]
    [InlineData("Russia", null, "ru", "+7")]
    [InlineData("Russia", "", "ru", "+7")]
    [InlineData("Russia", " ", "ru", "+7")]
    [InlineData("Russia", "Россия", null, "+7")]
    [InlineData("Russia", "Россия", "", "+7")]
    [InlineData("Russia", "Россия", " ", "+7")]
    [InlineData("Russia", "Россия", "ru", null)]
    [InlineData("Russia", "Россия", "rus", null)]
    [InlineData("Russia", "Россия", "ru", "")]
    [InlineData("Russia", "Россия", "ru", " ")]
    public async Task UpdateCountry_Fail_Test(
        string name, 
        string nameRu,
        string code,
        string phoneCode)
    {
        // Arrange
        string existingName      = "Russia";
        string existingNameRu    = "Россия";
        string existingCode      = "ru";
        string existingPhoneCode = "+7";
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = Delivery.Domain.Entities.Country.Create(
            name: existingName,
            nameRu: existingNameRu,
            code: existingCode,
            phoneCode: existingPhoneCode,
            now: now);
        
        // Assert
        Assert.Throws<DomainException>(() => 
            result.Update(
                name: name,
                nameRu: nameRu,
                code: code,
                phoneCode: phoneCode,
                now: now));
    }
    
    [Theory]
    [InlineData("r", "+7")]
    [InlineData("rus", "+7")]
    [InlineData("ru", "+788")]
    public async Task UpdateCountry_FailOnCodeSymbols_Test(string newCode, string newPhoneCode)
    {
        // Arrange
        string name          = "Russia";
        string nameRu        = "Россия";
        string code          = "ru";
        string phoneCode     = "+7";
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = Delivery.Domain.Entities.Country.Create(
            name: name,
            nameRu: nameRu,
            code: code,
            phoneCode: phoneCode,
            now: now);
        
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            result.Update(
                name: name,
                nameRu: nameRu,
                code: newCode,
                phoneCode: newPhoneCode,
                now: now));
    }
    
    [Fact]
    public async Task UpdateCountry_FailOnAlreadyDeleted_Test()
    {
        // Arrange
        string name      = "Russia";
        string nameRu    = "Россия";
        string code      = "ru";
        string phoneCode = "+7";
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = Delivery.Domain.Entities.Country.Create(
            name: name,
            nameRu: nameRu,
            code: code,
            phoneCode: phoneCode,
            now: now);
        
        result.Delete(now: now);
        
        // Act & Assert
        Assert.NotNull(result);
        Assert.True(result.IsDeleted);

        Assert.Throws<DomainException>(() => 
            result.Update(
                name: name,
                nameRu: nameRu,
                code: code,
                phoneCode: phoneCode,
                now: now));
    }

    [Fact]
    public async Task DeleteCountry_Success_Test()
    {
        // Arrange
        string name      = "Russia";
        string nameRu    = "Россия";
        string code      = "ru";
        string phoneCode = "+7";
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = Delivery.Domain.Entities.Country.Create(
            name: name,
            nameRu: nameRu,
            code: code,
            phoneCode: phoneCode,
            now: now);
        
        result.Delete(now: now);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsDeleted);
    }
    
    [Fact]
    public async Task DeleteCountry_FailOnAlreadyDeleted_Test()
    {
        // Arrange
        string name      = "Russia";
        string nameRu    = "Россия";
        string code      = "ru";
        string phoneCode = "+7";
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = Delivery.Domain.Entities.Country.Create(
            name: name,
            nameRu: nameRu,
            code: code,
            phoneCode: phoneCode,
            now: now);
        
        result.Delete(now: now);
        
        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsDeleted);

        Assert.Throws<DomainException>(() => result.Delete(now: now));
    }
}