using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.VariantDetails;

public class ChangeModelFeaturesTests : DetailTestBase
{
    private const string MaxModelFeaturesLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";
    
    [Theory]
    [InlineData("New sample sample model features")]
    [InlineData(" New sample sample model features ")]
    public void Should_Raise_ModelFeaturesUpdated_Event(
        string newModelFeatures)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New sample sample model features";
        
        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeModelFeatures(
            now: now,
            modelFeatures: newModelFeatures);
        
        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsModelFeaturesUpdated>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.ModelFeatures);
        Assert.Equal(now, result.OccurredAt);
        
        // Assert: state
        Assert.Equal(expectedResult, detail.ModelFeatures);
        Assert.Equal(now, detail.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_ModelFeaturesTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeModelFeatures(
            now: now,
            modelFeatures: detail.ModelFeatures);
        
        Assert.Empty(detail.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxModelFeaturesLength)]
    public void Should_Throw_Where_ModelFeaturesIsIncorrect(
        string modelFeatures)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        
        var detail = CreateDetail(); 
        detail.UncommittedEventsClear(); 

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            detail.ChangeModelFeatures(
                now: now,
                modelFeatures: modelFeatures));
    }
}