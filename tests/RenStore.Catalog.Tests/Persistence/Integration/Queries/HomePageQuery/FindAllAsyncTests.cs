/*using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Media;
using RenStore.Catalog.Domain.Aggregates.Variant;
using RenStore.Catalog.Domain.Enums;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.Write.Projections;
using RenStore.Catalog.Persistence.Write.Repositories.Postgresql;
using RenStore.Catalog.Tests.Persistence.Integration.Repositories;
using RenStore.SharedKernal.Domain.Enums;

namespace RenStore.Catalog.Tests.Persistence.Integration.Queries.HomePageQuery;

public class FindAllAsyncTests : IAsyncLifetime
{
    private CatalogDbContext _context;
    
    public async Task InitializeAsync()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: CatalogRepositoryTestsBase
                .BuildConnectionString(Guid.NewGuid()))
            .Options;

        _context = new CatalogDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
    }
    
    [Fact]
    public async Task Should_Load_Items_From_Postgres()
    {
        // Arrange: variant
        var now = DateTimeOffset.UtcNow;
        var productId = Guid.NewGuid();
        var colorId = 32432;
        var name = "Sample product variant name";
        var sizeType = SizeType.Clothes;
        var sizeSystem = SizeSystem.EU;
        var article = 42422342;
        var url = "https://newstringurl/catalog";
        var priceAmount = 5555;
        
        // Arrange: image
        var nowImage = DateTimeOffset.UtcNow.AddMinutes(2);
        var originalFileName = "testimg.jpg";
        var storagePath = "test/storage/path";
        var fileSizeBytes = 4235334;
        var isMain = false;
        short sortOrder = 1;
        var weight = 432;
        var height = 4322;

        var variant = ProductVariant.Create(
            now: nowImage,
            productId: productId,
            colorId: colorId,
            name: name,
            sizeType: sizeType,
            sizeSystem: sizeSystem,
            article: article,
            url: url);
        
        var sizeId = variant.AddSize(
            now: nowImage,
            letterSize: LetterSize.M);
        
        variant.AddPriceToSize(
            now: nowImage,
            validFrom: nowImage,
            amount: priceAmount,
            currency: Currency.RUB,
            sizeId: sizeId);

        var image = VariantImage.Create(
            now: nowImage,
            originalFileName: originalFileName,
            variantId: variant.Id,
            storagePath: storagePath,
            fileSizeBytes: fileSizeBytes,
            isMain: isMain,
            sortOrder: sortOrder,
            weight: weight,
            height: height);
        
        var eventStoreMock = new Mock<IEventStore>();

        var productVariantRepository = new ProductVariantProjection(
            context: _context, 
            eventStore: eventStoreMock.Object);
        
        var productImageRepository = new VariantImageProjection(
            _context, eventStoreMock.Object);
        
        await productVariantRepository.AddAsync(variant, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        await productImageRepository.AddAsync(image, CancellationToken.None);
        await _context.SaveChangesAsync();
        
        var query = new Catalog.Persistence.Read.Queries.Postgresql
            .HomePageQuery(NullLogger<Catalog.Persistence.Read.Queries.Postgresql
                    .HomePageQuery>.Instance, 
                _context);

        // Act
        var result = await query.FindAllAsync(
            CancellationToken.None);

        // Assert
        Assert.Single(result);
        
        Assert.NotNull(result);
        Assert.Equal(variant.Id, result[0].Id);
        Assert.Equal(name, result[0].Name);
        Assert.Equal(article, result[0].Article);
        Assert.Equal(priceAmount, result[0].Amount);
        Assert.Equal(height, result[0].Height);
        Assert.Equal(weight, result[0].Weight);
        Assert.Equal(storagePath, result[0].StoragePath);
    }
    
    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}*/