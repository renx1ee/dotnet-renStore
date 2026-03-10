using RenStore.Catalog.Domain.Aggregates.Variant.Events.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.SharedKernal.Domain.Exceptions;

namespace RenStore.Catalog.Tests.Domain.UnitTets.Aggregates.ProductVariant;

public class CreateTests
{
    private const string MaxVariantName = 
        "name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name name ";

    private const string MaxVariantUrl = 
        "https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/https://url/";
    
    [Fact]
    public void Should_Raise_VariantCreated_Event()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var productId = Guid.NewGuid();
        var colorId = 1231;
        var name = "Sample product variant name";
        var article = 4232255;
        string url = "https://renstore/catallg/efwfw/fwfww";

        // Act
        var variant = RenStore.Catalog.Domain.Aggregates.Variant.ProductVariant
            .Create(
                now: now,
                productId: productId,
                colorId: colorId,
                name: name,
                sizeSystem: SizeSystem.EU,
                sizeType: SizeType.Clothes,
                article: article,
                url: url);

        var @event = Assert.Single(variant.GetUncommittedEvents());
        var result = Assert.IsType<VariantCreatedEvent>(@event);
        
        // Assert: event
        Assert.Equal(now, result.OccurredAt);
        Assert.Equal(productId, result.ProductId);
        Assert.Equal(colorId, result.ColorId);
        Assert.Equal(name, result.Name);
        Assert.Equal(url, result.Url);
        Assert.Equal(article, result.Article);
        
        // Assert: state
        Assert.Equal(variant.Id, result.VariantId);
        Assert.NotEqual(Guid.Empty, result.VariantId);
        Assert.Equal(now, variant.CreatedAt);
        Assert.Equal(productId, variant.ProductId);
        Assert.Equal(colorId, variant.ColorId);
        Assert.Equal(name, variant.Name);
        Assert.Equal(url, variant.Url);
        Assert.Equal(article, variant.Article);
    }

    [Fact]
    public void Should_Throw_Where_ProductIdIsIncorrect()
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var productId = Guid.Empty;
        var colorId = 1231;
        var article = 4232255;
        var name = "Sample product variant name";
        string url = "https://renstore/catallg";

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            RenStore.Catalog.Domain.Aggregates.Variant.ProductVariant
                .Create(
                    now: now,
                    productId: productId,
                    colorId: colorId,
                    name: name,
                    sizeSystem: SizeSystem.EU,
                    sizeType: SizeType.Clothes,
                    article: article,
                    url: url));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Throw_Where_ColorIdIsIncorrect(
        int colorId)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var productId = Guid.NewGuid();
        var name = "Sample product variant name";
        string url = "https://renstore/catallg";
        var article = 4232255;

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            RenStore.Catalog.Domain.Aggregates.Variant.ProductVariant
                .Create(
                    now: now,
                    productId: productId,
                    colorId: colorId,
                    name: name,
                    sizeSystem: SizeSystem.EU,
                    sizeType: SizeType.Clothes,
                    article: article,
                    url: url));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Sample")]
    [InlineData(MaxVariantName)]
    public void Should_Throw_Where_NameIsIncorrect(
        string name)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var productId = Guid.NewGuid();
        var colorId = 1231;
        string url = "https://renstore/catallg";
        var article = 4232255;

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            RenStore.Catalog.Domain.Aggregates.Variant.ProductVariant
                .Create(
                    now: now,
                    productId: productId,
                    colorId: colorId,
                    name: name,
                    sizeSystem: SizeSystem.EU,
                    sizeType: SizeType.Clothes,
                    article: article,
                    url: url));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Sample")]
    [InlineData(MaxVariantUrl)]
    public void Should_Throw_Where_UrlIsIncorrect(
        string url)
    {
        // Arrange
        var now = DateTimeOffset.UtcNow;
        var productId = Guid.NewGuid();
        var colorId = 1231;
        var name = "Sample product variant name";
        var article = 4232255;

        // Act & Assert
        Assert.Throws<DomainException>(() =>
            RenStore.Catalog.Domain.Aggregates.Variant.ProductVariant
                .Create(
                    now: now,
                    productId: productId,
                    colorId: colorId,
                    name: name,
                    sizeSystem: SizeSystem.EU,
                    sizeType: SizeType.Clothes,
                    article: article,
                    url: url));
    }
}