using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Tests.UnitTets.Persistence.Repositories;

namespace RenStore.Catalog.Tests.Integration.Persistence.Repositories.CategoryRepository;

public class RemoveTests : IAsyncLifetime
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
    public async Task Should_Removed_Category_From_Postgres()
    {
        // Arrange
        var categoryName = "Clothes";
        var categoryNameRu = "Одежда";
        var description = "Sample testing category description";
        var now = DateTimeOffset.UtcNow;
        
        var category = Category.Create(
            name: categoryName,
            nameRu: categoryNameRu,
            description: description,
            now: now);
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(
                _context, 
                eventStoreMock.Object);
        
        var categoryId = await repository.AddAsync(category, CancellationToken.None);
        await _context.SaveChangesAsync();

        var existingCategory = await _context.Categories
            .Where(x => x.Id == categoryId)
            .ToListAsync();

        Assert.NotNull(existingCategory);
        Assert.Single(existingCategory);

        // Act
        repository.Remove(existingCategory[0]);
        await _context.SaveChangesAsync();

        // Assert
        var result = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == categoryId);
        
        Assert.Null(result);
    }
    
    [Fact]
    public async Task Should_Throw_When_Category_Is_Null()
    {
        // Arrange
        Category category = null;
        
        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(
                _context, 
                eventStoreMock.Object);

        // Act
        Assert.Throws<ArgumentNullException>(() => 
            repository.Remove(category));
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}