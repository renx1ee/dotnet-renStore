using RenStore.Catalog.Domain.Aggregates.Media.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.Media.Image;

public class ChangeDimensionTests : ImageTestBase
{
    [Theory]
    [InlineData(479, 1000)]
    [InlineData(500, 600)]
    public void Should_Raise_DimensionUpdated_Event(
        int weight,
        int height)
    {
        // Arrange
        var now = DateTimeOffset.Now;
        var image = CreateValidImage();
        image.UncommittedEventsClear();
        
        // Act
        image.ChangeDimension(
            weight: weight, 
            height: height, 
            now: now);

        var @event = Assert.Single(image.GetUncommittedEvents());
        var created = Assert.IsType<ImageDimensionUpdatedEvent>(@event);
        
        // Assert: event
        Assert.Equal(weight, created.Weight);
        Assert.Equal(height, created.Height);
        Assert.Equal(now, created.OccurredAt);
        Assert.NotEqual(Guid.Empty, created.ImageId);
        
        // Assert: state
        Assert.Equal(image.Id, created.ImageId);
        Assert.Equal(weight, image.Weight);
        Assert.Equal(height, image.Height);
        Assert.Equal(now, image.UpdatedAt);
    }
    
    [Theory]
    [InlineData(-1, 1000)]
    [InlineData(0, 1000)]
    [InlineData(49, 1000)]
    [InlineData(500, -1)]
    [InlineData(500, 0)]
    [InlineData(500, 5001)]
    public void Should_Throw_Where_ParametersAreWrong(
        int weight,
        int height)
    {
        // Arrange
        var now = DateTimeOffset.Now;

        var image = CreateValidImage();
        
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            image.ChangeDimension(
                weight: weight, 
                height: height, 
                now: now));
    }
    
    [Fact]
    public void Should_Throw_Where_ParametersAreSame()
    {
        // Arrange
        var now = DateTimeOffset.Now;

        var image = CreateValidImage();
        image.UncommittedEventsClear();
        
        // Act 
        image.ChangeDimension(
            weight: image.Weight,
            height: image.Height,
            now: now);

        // Assert
        Assert.Empty(image.GetUncommittedEvents());
    }
    
    [Fact]
    public void Should_Throw_Where_ImageIsAlreadyDeleted()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        var now = DateTimeOffset.Now;
        var weight = 500;
        var height = 600;

        var image = CreateValidImage();
        image.Delete(
            now: now,
            updatedById: updatedById,
            updatedByRole: updatedByRole);
        
        // Act & Assert
        Assert.Throws<DomainException>(() => 
            image.ChangeDimension(
                weight: weight, 
                height: height, 
                now: now));
    }
}