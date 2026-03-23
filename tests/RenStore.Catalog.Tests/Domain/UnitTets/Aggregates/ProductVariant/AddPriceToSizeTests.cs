using RenStore.Catalog.Domain.Aggregates.Variant.Events.Price;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public sealed class AddPriceToSizeTests : ProductVariantTestBase
{
    [Fact]
    public void Should_Rise_Variant_Size_Added()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var letterSize = LetterSize.L;
        decimal amount = 1234;
        var currency = Currency.RUB;
        
        var variant = CreateValidProductVariant();
        
        // Act
        var sizeId = variant.AddSize(
            now: now,
            letterSize: letterSize);
        
        variant.UncommittedEventsClear();
        
        variant.AddPriceToSize(
            now: now,
            validFrom: now,
            amount: amount,
            currency: currency,
            sizeId: sizeId);
        
        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<PriceCreatedEvent>(@event);
        
        // Assert: event
        Assert.Equal(sizeId, result.SizeId);
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(currency, result.Currency);
        Assert.Equal(amount, result.PriceAmount);
        Assert.Equal(now, result.EffectiveFrom);
        
        // Assert: state
        Assert.Equal(now, variant.UpdatedAt);

        var prices = variant.Sizes
            .FirstOrDefault(x =>
                x.Id == sizeId)
            ?.Prices;

        Assert.Single(prices);
        Assert.Equal(amount, prices[0].Price.Amount);
        Assert.Equal(currency, prices[0].Price.Currency);
    }
    
    [Fact]
    public void Should_Throw_When_Variant_Size_Already_Deleted()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var letterSize = LetterSize.L;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        decimal amount = 1234;
        var currency = Currency.RUB;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        var sizeId = variant.AddSize(
            now: now,
            letterSize: letterSize);
        
        variant.RemoveSize(
            updatedByRole: updatedByRole,
            updatedById: updatedById,
            now: now, 
            sizeId: sizeId);
        
        Assert.Throws<DomainException>(() =>
            variant.AddPriceToSize(
                now: now,
                validFrom: now,
                amount: amount,
                currency: currency,
                sizeId: sizeId));
    }
    
    [Fact]
    public void Should_Throw_When_Size_Id_Is_Invalid()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var letterSize = LetterSize.L;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        decimal amount = 1234;
        var currency = Currency.RUB;
        var sizeId = Guid.Empty;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        
        Assert.Throws<DomainException>(() =>
            variant.AddPriceToSize(
                now: now,
                validFrom: now,
                amount: amount,
                currency: currency,
                sizeId: sizeId));
    }
    
    [Fact]
    public void Should_Throw_When_Variant_Already_Deleted()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var letterSize = LetterSize.L;
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        decimal amount = 1234;
        var currency = Currency.RUB;
        
        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        var sizeId = variant.AddSize(
            now: now,
            letterSize: letterSize);
            
        variant.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        Assert.Throws<DomainException>(() =>
            variant.AddPriceToSize(
                now: now,
                validFrom: now,
                amount: amount,
                currency: currency,
                sizeId: sizeId));
    }
}