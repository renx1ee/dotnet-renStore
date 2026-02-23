using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Repositories.CategoryRepository;

public class AddAsyncTests : IAsyncLifetime
{
    private static string _connectionString =
        $"Server=localhost;Port=5432;DataBase={Guid.NewGuid()}; User Id=re;Password=postgres;Include Error Detail=True";
    
    private CatalogDbContext _context;
    
    [Fact]
    public async Task Should_Save_Category_To_Postgres()
    {
        // Arrange
        var categoryName = "Clothes";
        var categoryNameRu = "Одежда";
        var description = "Sample testing category description";
        var now = DateTimeOffset.UtcNow;
        
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: _connectionString)
            .Options;

        _context = new CatalogDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();
        /*await context.Database.MigrateAsync();*/

        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Persistence.Write.Repositories.Postgresql
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
        
        var options = new DbContextOptionsBuilder<CatalogDbContext>()
            .UseNpgsql(connectionString: _connectionString)
            .Options;

        _context = new CatalogDbContext(options);
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        var eventStoreMock = new Mock<IEventStore>();
        
        var repository = new Persistence.Write.Repositories.Postgresql
            .CategoryRepository(
                _context, 
                eventStoreMock.Object);

        // Act % Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await repository.AddAsync(
                category: category!, 
                CancellationToken.None));
    }
    
    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}