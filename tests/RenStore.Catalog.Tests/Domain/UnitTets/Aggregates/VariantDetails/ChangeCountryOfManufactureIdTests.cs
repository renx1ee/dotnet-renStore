using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.VariantDetails;

public class ChangeCountryOfManufactureIdTests : DetailTestBase
{
    [Theory]
    [InlineData(12)]
    [InlineData(52)]
    public void Should_Raise_CountryOfManufactureIdUpdated_Event(
        int countryId)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeCountryOfManufactureId(
            now: now,
            countryOfManufactureId: countryId);

        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsCountryOfManufactureIdUpdated>(@event);

        // Assert: event
        Assert.Equal(countryId, result.CountryOfManufactureId);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(countryId, detail.CountryOfManufactureId);
        Assert.Equal(now, detail.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_CountryOfManufactureIdTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeCountryOfManufactureId(
            now: now,
            countryOfManufactureId: detail.CountryOfManufactureId);

        Assert.Empty(detail.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Should_Throw_NoRise_Where_IncorrectCountryId(
        int countryId)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            detail.ChangeCountryOfManufactureId(
                now: now,
                countryOfManufactureId: countryId));
    }
}