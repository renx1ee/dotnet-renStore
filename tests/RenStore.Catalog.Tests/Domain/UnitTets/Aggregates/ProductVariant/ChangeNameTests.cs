using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public class ChangeNameTests : ProductVariantTestBase
{
    private const string MaxProductName = 
        "name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name ";
    
    [Fact]
    public void Should_Raise_NameUpdated_Event()
    {
        // Arrange
        var name = "new name name name name name name name";
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        variant.ChangeName( 
            name: name, 
            now: now); 

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantNameUpdated>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(name, result.Name);

        // Assert: state
        Assert.Equal(variant.Id, result.VariantId);
        Assert.Equal(now, variant.UpdatedAt);
        Assert.Equal(name, variant.Name);
    }
    
    [Fact]
    public void Should_NoRaise_When_NameTheSame()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        variant.ChangeName( 
            name: variant.Name, 
            now: now); 

        Assert.Empty(variant.GetUncommittedEvents());
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Name")]
    [InlineData(MaxProductName)] 
    public void Should_Throw_When_NameIsIncorrect(
        string name)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        variant.UncommittedEventsClear();

        // Act
        Assert.Throws<DomainException>(() => 
            variant.ChangeName(
                name: name,
                now: now));
    }
    
    [Fact]
    public void Should_Throw_When_VariantIsAlreadyDeleted()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;

        var variant = CreateValidProductVariant();
        variant.Delete(now);

        Assert.Throws<DomainException>(() =>
            variant.ChangeName( 
            name: variant.Name, 
            now: now));
    }
}