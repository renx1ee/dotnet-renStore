using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.VariantDetails;

public class 
    ChangeDetailDescriptionTests : DetailTestBase
{
    private const string MaxDescriptionLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";
    
    [Theory]
    [InlineData("New sample sample Description")]
    [InlineData(" New sample sample Description ")]
    public void Should_Raise_DescriptionUpdated_Event(
        string newDescription)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New sample sample Description";
        
        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeDescription(
            now: now,
            description: newDescription);
        
        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsDescriptionUpdated>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.Description);
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(expectedResult, detail.Description);
        Assert.Equal(now, detail.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_DescriptionTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeDescription(
            now: now,
            description: detail.Description);
        
        Assert.Empty(detail.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New Description")]
    [InlineData(MaxDescriptionLength)]
    public void Should_Throw_Where_DescriptionIsIncorrect(
        string newDescription)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var detail = CreateDetail();
        detail.UncommittedEventsClear(); 

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            detail.ChangeDescription(
                now: now,
                description: newDescription));
    }
}