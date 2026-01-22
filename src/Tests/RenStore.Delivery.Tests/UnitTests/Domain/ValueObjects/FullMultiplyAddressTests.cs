using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.ValueObjects;

namespace RenStore.Delivery.Tests.UnitTests.Domain.ValueObjects;

public sealed class FullMultiplyAddressTests
{
    private readonly Country _coutnry;
    private readonly City _city;

    public FullMultiplyAddressTests()
    {
        var result = _coutnry = Country.Create(
            name: "Key", 
            nameRu: "NameRu", 
            code: "ru", 
            phoneCode: "+7",
            now: DateTimeOffset.UtcNow);
        
        _city = City.Create(
            name: "City", 
            nameRu: "City", 
            countryId: 1,
            now: DateTimeOffset.UtcNow);
    }
    
    [Theory]
    [InlineData("houseCode", "apartmentNumber", "entrance", 1)]
    [InlineData("", "apartmentNumber", "entrance", 1)]
    [InlineData("houseCode", "", "entrance", 1)]
    [InlineData("houseCode", "apartmentNumber", "", 1)]
    [InlineData("houseCode", "apartmentNumber", "entrance")]
    [InlineData("", "", "", null)]
    public async Task BuildFullMultiplyAddress_WithNoRequiredData_Success_Test(
        string houseCode, 
        string apartmentNumber, 
        string entrance, 
        int? floor = null)
    {
        // Arrange
        string street = "street";
        string buildingNum = "buildingNumber";
        
        // Act
        var result = FullMultiplyAddress.BuildFull(
            country: _coutnry,
            city: _city,
            street: street,
            buildingNumber: buildingNum,
            houseCode: houseCode,
            apartmentNumber: apartmentNumber,
            entrance: entrance,
            floor: floor);
        
        // Assert
        Assert.NotNull(result);
        Assert.NotEqual(string.Empty, result.English);
        Assert.NotEqual(string.Empty, result.Russian);
    }
    
    [Fact]
    public async Task BuildFullMultiplyAddress_FailWithNullCountry_ThrowArgumentNullException_Test()
    {
        Assert.Throws<ArgumentNullException>(() => FullMultiplyAddress.BuildFull(
            country: null,
            city: _city,
            street: "street",
            buildingNumber: "buildNum",
            houseCode: "3332",
            apartmentNumber: "425",
            entrance: "423",
            floor: 1));
    }
    
    [Fact]
    public async Task BuildFullMultiplyAddress_FailWithNullCity_ThrowArgumentNullException_Test()
    {
        Assert.Throws<ArgumentNullException>(() => FullMultiplyAddress.BuildFull(
            country: _coutnry,
            city: null,
            street: "street",
            buildingNumber: "buildNum",
            houseCode: "3332",
            apartmentNumber: "425",
            entrance: "423",
            floor: 1));
    }
    
    [Theory]
    [InlineData(null, "buildingNumber")]
    [InlineData("", "buildingNumber")]
    [InlineData(" ", "buildingNumber")]
    [InlineData("street", null)]
    [InlineData("street", "")]
    [InlineData("street", " ")]
    public async Task BuildFullMultiplyAddress_FailOnEmptyRequiredParams_Test(string street, string buildingNumber)
    {
        Assert.Throws<ArgumentNullException>(() => FullMultiplyAddress.BuildFull(
            country: _coutnry,
            city: _city,
            street: street,
            buildingNumber: buildingNumber,
            houseCode: "3332",
            apartmentNumber: "425",
            entrance: "423",
            floor: 1));
    }
}