using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Repositories.CategoryRepository;

public class RemoveTests : IAsyncLifetime
{
    private static string _connectionString =
        $"Server=localhost;Port=5432;DataBase={Guid.NewGuid()}; User Id=re;Password=postgres;Include Error Detail=True";

    private CatalogDbContext _context;
    
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

        // Act
        Assert.Throws<ArgumentNullException>(() => 
            repository.Remove(category));
    }
    
    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}