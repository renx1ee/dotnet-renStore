using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Application.Abstractions;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;

namespace RenStore.Catalog.Tests.UnitTets.Domain.Repositories.CategoryRepository;

public class AddRangeAsyncTests : IAsyncLifetime
{
    private static string _connectionString =
        $"Server=localhost;Port=5432;DataBase={Guid.NewGuid()}; User Id=re;Password=postgres;Include Error Detail=True";

    private CatalogDbContext _context;
    
    [Fact]
    public async Task Should_Save_Categories_To_Postgres()
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
        
        // Act
        await repository.AddRangeAsync(list, CancellationToken.None);
        await _context.SaveChangesAsync();

        // Assert
        var category1 = await _context.Categories.
            FirstOrDefaultAsync(x => x.Id == list[0].Id);
        
        var category2 = await _context.Categories.
            FirstOrDefaultAsync(x => x.Id == list[1].Id);
        
        Assert.Equal(2, await _context.Categories.CountAsync());
        
        Assert.NotNull(category1);
        Assert.Equal(category1Name, category1.Name);
        Assert.Equal(category1NameRu, category1.NameRu);
        Assert.Equal(description1, category1.Description);
        Assert.Equal(now1, category1.CreatedAt);
        
        Assert.NotNull(category2);
        Assert.Equal(category2Name, category2.Name);
        Assert.Equal(category2NameRu, category2.NameRu);
        Assert.Equal(description2, category2.Description);
        Assert.Equal(now2, category2.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Throw_When_CategoriesAreNull()
    {
        // Arrange
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

         List<Category> list = null;
        
        // Act
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await repository.AddRangeAsync(list, CancellationToken.None));
    }
    
    [Fact]
    public async Task Should_Not_Save_When_Categories_List_Count_0()
    {
        // Arrange
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

        var list = new List<Category>();
        
        // Act
        await repository.AddRangeAsync(list, CancellationToken.None);
        
        // Assert
        Assert.Equal(0, await _context.Categories.CountAsync());
    }

    public async Task InitializeAsync()
    {
    }

    public async Task DisposeAsync()
    {
        await _context.Database.EnsureDeletedAsync();
    }
}