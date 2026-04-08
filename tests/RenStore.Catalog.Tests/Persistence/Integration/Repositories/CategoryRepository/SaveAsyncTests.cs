using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;
using IMediator = MediatR.IMediator;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.CategoryRepository;

[Collection("sequential")]
public sealed class SaveAsyncTests : IAsyncLifetime
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
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var categoryRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(eventStore, mediatorMock.Object);

        var category = Category.Create(
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            isActive: true,
            name: categoryName,
            nameRu: categoryNameRu,
            description: description,
            now: now);

        // Act
        await categoryRepository.SaveAsync(category, CancellationToken.None);

        // Assert
        var categoryEvents = await _context.Events
            .Where(x => x.AggregateId == category.Id)
            .ToListAsync();
        
        Assert.NotEmpty(categoryEvents);

        var savedCategory = await categoryRepository.GetAsync(
            category.Id, CancellationToken.None);
        
        Assert.Equal(categoryName, savedCategory.Name);
        Assert.Equal(category.Id, savedCategory.Id);
        Assert.Equal(categoryNameRu, savedCategory.NameRu);
        Assert.Equal(now, savedCategory.CreatedAt);
    }
    
    [Fact]
    public async Task Should_Throw_ArgumentNullException_When_CategoryIsNull()
    {
        // Arrange
        Category category = null;

        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<IMediator>();
        
        var categoryRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(eventStore, mediatorMock.Object);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            await categoryRepository.SaveAsync(
                category: category!, 
                CancellationToken.None));
    }

    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}