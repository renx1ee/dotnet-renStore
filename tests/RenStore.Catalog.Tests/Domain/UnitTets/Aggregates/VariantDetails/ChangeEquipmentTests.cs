using RenStore.Catalog.Domain.Aggregates.VariantDetails.Events;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.VariantDetails;

public class ChangeEquipmentTests : DetailTestBase
{
    private const string MaxEquipmentLength =
        "New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description New DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew DescriptionNew Description";

    [Theory]
    [InlineData("New Equipment")]
    [InlineData(" New Equipment ")]
    public void Should_Raise_EquipmentUpdated_Event(
        string newEquipment)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var expectedResult = "New Equipment";

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act
        detail.ChangeEquipment(
            now: now,
            equipment: newEquipment);

        var @event = Assert.Single(detail.GetUncommittedEvents());
        var result = Assert.IsType<VariantDetailsEquipmentUpdated>(@event);

        // Assert: event
        Assert.Equal(expectedResult, result.Equipment);
        Assert.Equal(now, result.OccurredAt);

        // Assert: state
        Assert.Equal(expectedResult, detail.Equipment);
        Assert.Equal(now, detail.UpdatedAt);
    }

    [Fact]
    public void Should_NoRise_Where_EquipmentTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        detail.ChangeEquipment(
            now: now,
            equipment: detail.Equipment);

        Assert.Empty(detail.GetUncommittedEvents());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("New")]
    [InlineData(MaxEquipmentLength)]
    public void Should_Throw_Where_EquipmentIsIncorrect(
        string equipment)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var detail = CreateDetail();
        detail.UncommittedEventsClear();

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            detail.ChangeEquipment(
                now: now,
                equipment: equipment));
    }
}