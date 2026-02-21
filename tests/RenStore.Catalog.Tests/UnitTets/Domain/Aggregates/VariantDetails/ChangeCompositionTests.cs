using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Aggregates.VariantDetails;

public class ChangeCompositionTests : DetailTestBase
{
    private const string MaxCompositionLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";

    [Theory]
    [InlineData("New Composition")]
    [InlineData(" New Composition ")]
    public void Should_Raise_CompositionUpdated_Event(
        string newComposition)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New Composition";

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeComposition(
            now: now,
            composition: newComposition);

        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsCompositionUpdated>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.Composition);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(expectedResult, detail.Composition);
        Assert.Equal(now, detail.UpdatedAt);
    }

    [Fact]
    public void Should_NoRise_Where_CompositionTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeComposition(
            now: now,
            composition: detail.Composition);

        Assert.Empty(detail.GetUncommittedEvents());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxCompositionLength)]
    public void Should_Throw_Where_CompositionIsIncorrect(
        string composition)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            detail.ChangeComposition(
                now: now,
                composition: composition));
    }
}