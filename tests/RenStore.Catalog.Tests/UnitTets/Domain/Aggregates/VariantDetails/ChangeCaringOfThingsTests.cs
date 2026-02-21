using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.VariantDetails;

public class ChangeCaringOfThingsTests : DetailTestBase
{
    private const string MaxCaringOfThingsLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";

    [Theory]
    [InlineData("New CaringOfThings")]
    [InlineData(" New CaringOfThings ")]
    public void Should_Raise_CaringOfThingsUpdated_Event(
        string newCaringOfThings)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New CaringOfThings";

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeCaringOfThings(
            now: now,
            caringOfThings: newCaringOfThings);

        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsCaringOfThingsUpdated>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.CaringOfThings);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(expectedResult, detail.CaringOfThings);
        Assert.Equal(now, detail.UpdatedAt);
    }

    [Fact]
    public void Should_NoRise_Where_CaringOfThingsTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeCaringOfThings(
            now: now,
            caringOfThings: detail.CaringOfThings);

        Assert.Empty(detail.GetUncommittedEvents());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxCaringOfThingsLength)]
    public void Should_Throw_Where_CaringOfThingsIsIncorrect(
        string composition)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            detail.ChangeCaringOfThings(
                now: now,
                caringOfThings: composition));
    }
}