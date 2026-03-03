using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.Catalog.Domain.Enums;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.VariantDetails;

public class ChangeTypeOfPackingTests : DetailTestBase
{
    [Theory]
    [InlineData(TypeOfPacking.Package)]
    public void Should_Raise_TypeOfPackingUpdated_Event(
        TypeOfPacking newTypeOfPacking)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New Equipment";

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeTypeOfPacking(
            now: now,
            typeOfPacking: newTypeOfPacking);

        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsTypeOfPackingUpdated>(@event);

        // Assert: event
        Assert.Equal(newTypeOfPacking, result.TypeOfPacking);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(newTypeOfPacking, detail.TypeOfPacking);
        Assert.Equal(now, detail.UpdatedAt);
    }
    
    [Fact]
    public void Should_NoRise_Where_TypeOfPackingTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeTypeOfPacking(
            now: now,
            typeOfPacking: detail.TypeOfPacking!.Value);

        Assert.Empty(detail.GetUncommittedEvents());
    }
}