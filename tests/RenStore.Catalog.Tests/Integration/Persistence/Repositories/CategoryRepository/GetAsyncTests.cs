using Microsoft.EntityFrameworkCore;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;
using RenStore.Catalog.Tests.UnitTets.Persistence.Repositories;
using Xunit.Abstractions;

namespace RenStore.Catalog.Tests.Integration.Persistence.Repositories.CategoryRepository;

public class GetAsyncTests : IAsyncLifetime
{
    private readonly ITestOutputHelper testOutputHelper;

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

    public GetAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }
    
    // TODO: firstly, you need to create tests for the LoasAsync in the SqlEventStore 
    [Fact]
    public async Task Should_Getting_Categories_From_Postgres()
    {
        // Arrange
        var now1 = DateTimeOffset.UtcNow;
        var category1Name = "Clothes";
        var category1NameRu = "Одежда";
        var description1 = "Sample testing category description";
        
        var now2 = DateTimeOffset.UtcNow;
        var category2Name = "Shoes";
        var category2NameRu = "Обувь";
        var description2 = "Sample testing category description";

        var eventStore = new SqlEventStore(_context);
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(_context, eventStore);

        var list = new List<Category>()
        {
            Category.Create(
                name: category1Name,
                nameRu: category1NameRu,
                description: description1,
                now: now1),
            
            Category.Create(
                name: category2Name,
                nameRu: category2NameRu,
                description: description2,
                now: now2)
        };
        
        testOutputHelper.WriteLine($"Category1 ID before save: {list[0].Id}");
        testOutputHelper.WriteLine($"Category2 ID before save: {list[1].Id}");
        
        await repository.AddRangeAsync(list, CancellationToken.None);
        var savedResult = await _context.SaveChangesAsync();
        
        testOutputHelper.WriteLine($"SaveChangesAsync result: {savedResult} entities saved");
        
        // Act
        var existingCategory1 = await repository
            .GetAsync(
                id: list[0].Id,
                cancellationToken: CancellationToken.None);
        
        var existingCategory2 = await repository
            .GetAsync(
                id: list[1].Id,
                cancellationToken: CancellationToken.None);
        
        // Assert
        /*Assert.NotNull(existingCategory1); // TODO:
        Assert.Equal(now1, existingCategory1.CreatedAt);
        Assert.Equal(category1Name, existingCategory1.Name);
        Assert.Equal(category1NameRu, existingCategory1.NameRu);
        Assert.Equal(description1, existingCategory1.Description);
        
        Assert.NotNull(existingCategory2);
        Assert.Equal(now2, existingCategory2.CreatedAt);
        Assert.Equal(category2Name, existingCategory2.Name);
        Assert.Equal(category2NameRu, existingCategory2.NameRu);
        Assert.Equal(description2, existingCategory2.Description);*/
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}