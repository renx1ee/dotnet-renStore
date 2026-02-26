using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Persistence.Repositories.CategoryRepository;

public class AddAsyncTests : IAsyncLifetime
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
    public async Task Should_Save_Category_To_Postgres()
    {
        // Arrange
        var categoryName = "Clothes";
        var categoryNameRu = "Одежда";
        var description = "Sample testing category description";
        var now = DateTimeOffset.UtcNow;

        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(
                _context, 
                eventStoreMock.Object);

        var category = Category.Create(
            name: categoryName,
            nameRu: categoryNameRu,
            description: description,
            now: now);

        // Act
        var categoryId = await repository.AddAsync(category, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var savedCategory = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == category.Id);

        Assert.NotNull(savedCategory);
        Assert.Equal(categoryName, savedCategory.Name);
        Assert.Equal(categoryId, savedCategory.Id);
        Assert.Equal(categoryNameRu, savedCategory.NameRu);
        Assert.Equal(now, savedCategory.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Throw_When_CategoryIsNull()
    {
        // Arrange
        Category category = null;

        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(
                _context, 
                eventStoreMock.Object);

        // Act % Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await repository.AddAsync(
                category: category!, 
                CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        if(_context != null)
         await _context.Database.EnsureDeletedAsync();
    }
}