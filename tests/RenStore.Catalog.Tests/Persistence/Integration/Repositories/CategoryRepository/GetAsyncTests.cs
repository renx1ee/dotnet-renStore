using Microsoft.EntityFrameworkCore;
using Moq;
using RenStore.Catalog.Domain.Aggregates.Category;
using RenStore.Catalog.Persistence;
using RenStore.Catalog.Persistence.EventStore;
using Xunit.Abstractions;

namespace RenStore.Catalog.Tests.Persistence.Integration.Repositories.CategoryRepository;

[Collection("sequential")]
public sealed class GetAsyncTests : IAsyncLifetime
{
    private CatalogDbContext _context;
    private readonly ITestOutputHelper testOutputHelper;
    
    public GetAsyncTests(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }
    
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
    public async Task Should_Getting_Categories_From_Postgres()
    {
        // Arrange
        var updatedById = Guid.NewGuid();
        var updatedByRole = "Admin";
        
        var now1 = DateTimeOffset.UtcNow;
        var category1Name = "Clothes";
        var category1NameRu = "Одежда";
        var description1 = "Sample testing category description";
        
        var now2 = DateTimeOffset.UtcNow;
        var category2Name = "Shoes";
        var category2NameRu = "Обувь";
        var description2 = "Sample testing category description";

        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<MediatR.IMediator>();
        
        var categoryRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(eventStore, mediatorMock.Object);

        var category1 = Category.Create(
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            isActive: true,
            name: category1Name,
            nameRu: category1NameRu,
            description: description1,
            now: now1);
        
        var category2 = Category.Create(
            updatedById: updatedById,
            updatedByRole: updatedByRole,
            isActive: false,
            name: category2Name,
            nameRu: category2NameRu,
            description: description2,
            now: now2);
        
        
        testOutputHelper.WriteLine($"Category1 ID before save: {category1.Id}");
        testOutputHelper.WriteLine($"Category2 ID before save: {category2.Id}");
        
        await categoryRepository.SaveAsync(category1, CancellationToken.None);
        await categoryRepository.SaveAsync(category2, CancellationToken.None);
        
        // Act
        var existingCategory1 = await categoryRepository
            .GetAsync(
                id: category1.Id,
                cancellationToken: CancellationToken.None);
        
        var existingCategory2 = await categoryRepository
            .GetAsync(
                id: category2.Id,
                cancellationToken: CancellationToken.None);
        
        // Assert
        Assert.NotNull(existingCategory1); // TODO:
        Assert.Equal(now1, existingCategory1.CreatedAt);
        Assert.Equal(category1Name, existingCategory1.Name);
        Assert.Equal(category1NameRu, existingCategory1.NameRu);
        Assert.Equal(description1, existingCategory1.Description);
        
        Assert.NotNull(existingCategory2);
        Assert.Equal(now2, existingCategory2.CreatedAt);
        Assert.Equal(category2Name, existingCategory2.Name);
        Assert.Equal(category2NameRu, existingCategory2.NameRu);
        Assert.Equal(description2, existingCategory2.Description);
    }
    
    [Fact]
    public async Task Should_Return_Null_When_CategoryNotFound()
    {
        var eventStore = new SqlEventStore(_context);
        var mediatorMock = new Mock<MediatR.IMediator>();
        
        var categoryRepository = new Catalog.Persistence.Write.Repositories.Postgresql
            .CategoryRepository(eventStore, mediatorMock.Object);
        
        var result = await categoryRepository.GetAsync(
            Guid.NewGuid(), CancellationToken.None);
    
        Assert.Null(result);
    }

    public async Task DisposeAsync()
    {
        if(_context != null)
            await _context.Database.EnsureDeletedAsync();
    }
}