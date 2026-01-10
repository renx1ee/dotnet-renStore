using RenStore.Delivery.Domain.Entities;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities;

public class SortingCenterTests
{
    [Fact]
    public async Task CreateSortingCenter_Success_Test()
    {
        // Arrange
        string code = "QWERTy1";
        Guid addressId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = SortingCenter.Create(
            code: code,
            addressId: addressId,
            now: now);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(code, result.Code);
        Assert.Equal(addressId, result.AddressId);
        Assert.Equal(now, result.CreatedAt);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("1")]
    public async Task CreateSortingCenter_FailOnWrongCode_Test(
        string code)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var addressId = Guid.NewGuid();

        // Act & Assert
        Assert.Throws<DomainException>(
            () => SortingCenter.Create(
                code: code,
                addressId: addressId,
                now: now));
    }
    
    [Fact]
    public async Task CreateSortingCenter_FailOnWrongAddressId_Test()
    {
        // Arrange
        string code = "faewfew";
        var now = DateTimeOffset.UtcNow;
        var addressId = Guid.Empty;

        // Act & Assert
        Assert.Throws<DomainException>(
            () => SortingCenter.Create(
                code: code,
                addressId: addressId,
                now: now));
    }
    
    [Fact]
    public async Task DeleteSortingCenter_Success_Test()
    {
        // Arrange
        string code = "QWERTy1";
        Guid addressId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = SortingCenter.Create(
            code: code,
            addressId: addressId,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(code, result.Code);
        Assert.Equal(addressId, result.AddressId);
        Assert.Equal(now, result.CreatedAt);
        
        result.Delete(now);
        
        // Assert
        Assert.True(result.IsDeleted);
    }
    
    [Fact]
    public async Task DeleteSortingCenter_FailOnAlreadyDeleted_Test()
    {
        // Arrange
        string code = "QWERTy1";
        Guid addressId = Guid.NewGuid();
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = SortingCenter.Create(
            code: code,
            addressId: addressId,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(code, result.Code);
        Assert.Equal(addressId, result.AddressId);
        Assert.Equal(now, result.CreatedAt);
        
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(
            () => result.Delete(now));
    }
}