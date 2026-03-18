/*using RenStore.Catalog.Domain.Aggregates.Variant.Events.Deteils;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.VariantDetails;

public class ChangeDecorativeElementsTests : DetailTestBase
{
    private const string MaxDecorativeElementsLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";
    
    [Theory]
    [InlineData("New decorative elements")]
    [InlineData(" New decorative elements ")]
    public void Should_Raise_DecorativeElementsUpdated_Event(
        string newDecorativeElements)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New decorative elements";
        
        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeDecorativeElements(
            now: now,
            decorativeElements: newDecorativeElements);
        
        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsDecorativeElementsUpdated>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.DecorativeElements);
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(expectedResult, detail.DecorativeElements);
        Assert.Equal(now, detail.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_DecorativeElementsTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeDecorativeElements(
            now: now,
            decorativeElements: detail.DecorativeElements);
        
        Assert.Empty(detail.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxDecorativeElementsLength)]
    public void Should_Throw_Where_DecorativeElementsIsIncorrect(
        string decorativeElements)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var detail = CreateDetail(); 
        detail.UncommittedEventsClear(); 

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            detail.ChangeDecorativeElements(
                now: now,
                decorativeElements: decorativeElements));
    }
}*/