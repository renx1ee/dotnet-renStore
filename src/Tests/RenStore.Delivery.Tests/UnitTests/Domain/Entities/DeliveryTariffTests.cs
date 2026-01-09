using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums;
using RenStore.Delivery.Domain.ValueObjects;
using RenStore.SharedKernal.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;
using RenStore.SharedKernal.Domain.ValueObjects;

namespace RenStore.Delivery.Tests.UnitTests.Domain.Entities;

public sealed class DeliveryTariffTests
{
    [Fact]
    public async Task CreateDeliveryTariff_Success_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: DateTimeOffset.UtcNow);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
    }
    
    [Fact]
    public async Task CreateDeliveryTariff_FailOnNullPrice_Test()
    {
        // Arrange
        var weightLimitModel = new WeightLimitKg(100);
        string description = "aweafwfw";
        
        // Act & Assert
        Assert.Throws<DomainException>(() => DeliveryTariff.Create(
            price: null,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitModel,
            now: DateTimeOffset.UtcNow));
    }
    
    [Fact]
    public async Task CreateDeliveryTariff_FailOnNullWeightKgLimit_Test()
    {
        // Arrange
        var priceModel = new Price(100, Currency.RUB);
        string description = "aweafwfw";
        
        // Act & Assert
        Assert.Throws<DomainException>(() => DeliveryTariff.Create(
            price: priceModel,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: null,
            now: DateTimeOffset.UtcNow));
    }
    
    [Fact]
    public async Task ChangePriceOfDeliveryTariff_Success_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var newPrice = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.ChangePrice(newPrice, now);
        
        // Assert
        Assert.Equal(newPrice, result.Price);
    }
    
    [Fact]
    public async Task ChangePriceOfDeliveryTariff_FailOnNullPrice_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ChangePrice(null, now));
    }
    
    [Fact]
    public async Task ChangePriceOfDeliveryTariff_FailOnDeleted_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var newPrice = new Price(100, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ChangePrice(newPrice, now));
    }
    
    [Fact]
    public async Task ChangeWeightLimitKgOfDeliveryTariff_Success_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var newWeightLimitKg = new WeightLimitKg(100);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.ChangeWeightLimitKg(newWeightLimitKg, now);
        
        // Assert
        Assert.Equal(newWeightLimitKg, result.WeightLimitKg);
    }
    
    [Fact]
    public async Task ChangeWeightLimitKgOfDeliveryTariff_FailOnNullWeightLimitKg_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var newPrice = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ChangeWeightLimitKg(null, now));
    }
    
    [Fact]
    public async Task ChangeWeightLimitKgOfDeliveryTariff_FailOnDeleted_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var newWeightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ChangeWeightLimitKg(newWeightLimitKg, now));
    }
    
    [Fact]
    public async Task ChangeDescriptionOfDeliveryTariff_Success_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        string newDescription = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.ChangeDescription(newDescription, now);
        
        // Assert
        Assert.Equal(newDescription, result.Description);
    }
    
    [Fact]
    public async Task ChangeDescriptionOfDeliveryTariff_FailOnDeleted_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        string newDescription = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.ChangeDescription(newDescription, now));
    }
    
    [Fact]
    public async Task DeleteDeliveryTariff_Success_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.Delete(now);
        
        // Assert
        Assert.True(result.IsDeleted);
    }
    
    [Fact]
    public async Task DeleteDeliveryTariff_FailOnAlreadyDeleted_Test()
    {
        // Arrange
        string description = Guid.NewGuid().ToString();
        var price = new Price(1000, Currency.RUB);
        var weightLimitKg = new WeightLimitKg(100);
        var now = DateTimeOffset.UtcNow;
        
        // Act
        var result = DeliveryTariff.Create(
            price: price,
            type: DeliveryTariffType.Economy,
            description: description,
            weightLimitKg: weightLimitKg,
            now: now);
        
        Assert.NotNull(result);
        Assert.Equal(description, result.Description);
        Assert.Equal(price, result.Price);
        Assert.Equal(weightLimitKg, result.WeightLimitKg);
        
        result.Delete(now);
        
        // Assert
        Assert.Throws<DomainException>(() => result.Delete(now));
    }
}