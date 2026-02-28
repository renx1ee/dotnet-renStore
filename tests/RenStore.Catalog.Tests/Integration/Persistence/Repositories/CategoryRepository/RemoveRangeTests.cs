using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Tests.UnitTets.Persistence.Repositories;

namespace RenStore.Catalog.Tests.Integration.Persistence.Repositories.CategoryRepository;

public class RemoveRangeTests : IAsyncLifetime
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
    public async Task Should_Removed_Categories_From_Postgres()
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
        
        var categories = new List<Category>()
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

        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(
                _context, 
                eventStoreMock.Object);
        
        await repository.AddRangeAsync(categories, CancellationToken.None);
        await _context.SaveChangesAsync();

        var existingCategories = await _context.Categories.ToListAsync();

        Assert.NotNull(existingCategories);
        Assert.Equal(2, existingCategories.Count());

        // Act
        repository.RemoveRange(existingCategories);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Categories.ToListAsync();
        
        Assert.Empty(result);
    }
    
    [Fact]
    public async Task Should_Throw_When_Categories_Is_Null()
    {
        // Arrange
        List<Category> categories = null;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(
                _context, 
                eventStoreMock.Object);

        // Act
        Assert.Throws<ArgumentNullException>(() => 
            repository.RemoveRange(categories));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}