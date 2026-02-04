/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class CategoryRepositoryTests
{
    private ApplicationDbContext _context;
    private CategoryRepository _categoryRepository;

    #region Create Edit Delete

    [Fact]
    public async Task CreateCategoryAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int categoryId = 15356;
        var category = new Category()
        {
            Id = categoryId,
            Key = "Test",
            NormalizedName = "TEST",
            NameRu = "Тест",
            NormalizedNameRu = "ТЕСТ",
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        // Act
        await _categoryRepository.CreateAsync(category, CancellationToken.None);
        // Assert
        var result = await _context.Categories
            .FirstOrDefaultAsync(c => 
                c.Id == categoryId);
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
        Assert.Equal(category.Key, result.Key);
        Assert.Equal(category.NormalizedName, result.NormalizedName);
        Assert.Equal(category.NameRu, result.NameRu);
        Assert.Equal(category.NormalizedNameRu, result.NormalizedNameRu);
        Assert.Equal(category.Description, result.Description);
        Assert.Equal(category.IsActive, result.IsActive);
        Assert.Equal(category.CreatedAt, result.CreatedAt);
    }
    
    [Fact]
    public async Task CreateCategoryAsync_FailOnDuplicateName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int categoryId = 15356;
        var category = new Category()
        {
            Id = categoryId,
            Key = TestDataConstants.CategoryNameForGetting1,
            NormalizedName = TestDataConstants.CategoryNameForGetting1.ToUpper(),
            NameRu = "Тест",
            NormalizedNameRu = "ТЕСТ",
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.OccurredAt
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _categoryRepository
                .CreateAsync(
                    category: category, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateCategoryAsync_FailOnDuplicateNameRu_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int categoryId = 15356;
        var category = new Category()
        {
            Id = categoryId,
            Key = "wfkwej",
            NormalizedName = "lkwwfwawf".ToUpper(),
            NameRu = TestDataConstants.CategoryNameRuForGetting1,
            NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting1.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedAt = DateTime.OccurredAt
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _categoryRepository
                .CreateAsync(
                    category: category, 
                    CancellationToken.None));
        
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string categoryName = "Test";
        string categoryNameRu = "Test";
        string normalizedCategoryNameRu = "Test";
        string normalizedCategoryName = "Test";
        string categoryDescription = Guid.NewGuid().ToString();
        
        var existsCategory = await _context.Categories
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CategoryIdForUpdate);
        
        existsCategory.Key = categoryName;
        existsCategory.NameRu = categoryNameRu;
        existsCategory.NormalizedName = normalizedCategoryName;
        existsCategory.NormalizedNameRu = normalizedCategoryNameRu;
        existsCategory.Description = categoryDescription;
        // Act 
        await _categoryRepository.UpdateAsync(existsCategory, CancellationToken.None);
        
        // Assert
        var result = await _context.Categories
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CategoryIdForUpdate);
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.CategoryIdForUpdate, result.Id);
        Assert.Equal(categoryName, result.Key);
        Assert.Equal(categoryNameRu, result.NameRu);
        Assert.Equal(normalizedCategoryNameRu, result.NormalizedNameRu);
        Assert.Equal(normalizedCategoryName, result.NormalizedName);
        Assert.Equal(categoryDescription, result.Description);
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_FailOnDuplicateName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string categoryDescription = Guid.NewGuid().ToString();
        
        var existsCategory = await _context.Categories
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CategoryIdForUpdate);
        
        existsCategory.Key = TestDataConstants.CategoryNameForGetting1;
        existsCategory.NameRu = TestDataConstants.CategoryNameRuForGetting1;
        existsCategory.NormalizedName = TestDataConstants.CategoryNameForGetting1.ToUpper();
        existsCategory.NormalizedNameRu = TestDataConstants.CategoryNameRuForGetting1.ToUpper();
        existsCategory.Description = categoryDescription;
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _categoryRepository
                .UpdateAsync(
                    category: existsCategory, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateCategoryAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 1535656;
        var category = new Category()
        {
            Id = wrongId,
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _categoryRepository
                .UpdateAsync(
                    category: category, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteCategoryAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var existsCategory = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CategoryIdForDelete);
        Assert.NotNull(existsCategory);
        // Act
        await _categoryRepository.DeleteAsync(TestDataConstants.CategoryIdForDelete, CancellationToken.None);
        // Assert
        var result = await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CategoryIdForDelete);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteCategoryAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 15356;
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _categoryRepository
                .DeleteAsync(
                    id: wrongId, 
                    CancellationToken.None));
    }
    
    #endregion
    #region All
    [Fact]
    public async Task FindAllCategoriesAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository.FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(TestDataConstants.OverallCategoriesCount, categories.Count());
    }

    [Fact]
    public async Task FindAllCategoriesAsync_WithCountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            pageCount: 3);
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(3, categories.Count());
    }
    
    [Fact]
    public async Task FindAllCategoriesAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _categoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CategorySortBy.Id);
        var categories = result.ToList();
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(TestDataConstants.OverallCategoriesCount, categories.Count());
        Assert.Equal(TestDataConstants.CategoryIdForUpdate, categories[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForDelete, categories[1].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, categories[2].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting2, categories[3].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting3, categories[4].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting4, categories[5].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting5, categories[6].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, categories[7].Id);
    }
    
    [Fact]
    public async Task FindAllCategoriesAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _categoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CategorySortBy.Id);
        var categories = result.ToList();
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(TestDataConstants.OverallCategoriesCount, categories.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, categories[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, categories[1].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting5, categories[2].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting4, categories[3].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting3, categories[4].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting2, categories[5].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, categories[6].Id);
        Assert.Equal(TestDataConstants.CategoryIdForDelete, categories[7].Id);
        Assert.Equal(TestDataConstants.CategoryIdForUpdate, categories[8].Id);
    }
    
    [Fact]
    public async Task FindAllCategoriesAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _categoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CategorySortBy.Key);
        var categories = result.ToList();
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(TestDataConstants.OverallCategoriesCount, categories.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting5, categories[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForUpdate , categories[1].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, categories[2].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, categories[3].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, categories[4].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting3, categories[5].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting2, categories[6].Id);
        Assert.Equal(TestDataConstants.CategoryIdForDelete, categories[7].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting4, categories[8].Id);
    }
    
    [Fact]
    public async Task FindAllCategoriesAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _categoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CategorySortBy.Key);
        var categories = result.ToList();
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(TestDataConstants.OverallCategoriesCount, categories.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting4, categories[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForDelete, categories[1].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting2, categories[2].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting3, categories[3].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, categories[4].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, categories[5].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, categories[6].Id);
        Assert.Equal(TestDataConstants.CategoryIdForUpdate, categories[7].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting5, categories[8].Id);
    }
    
    [Fact]
    public async Task FindAllCategoriesAsync_SortByNameRu_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _categoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CategorySortBy.NameRu);
        var categories = result.ToList();
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(TestDataConstants.OverallCategoriesCount, categories.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting3, categories[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7 , categories[1].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6 , categories[2].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting5, categories[3].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting2, categories[4].Id);
        Assert.Equal(TestDataConstants.CategoryIdForDelete, categories[5].Id);
        Assert.Equal(TestDataConstants.CategoryIdForUpdate, categories[6].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting4, categories[7].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, categories[8].Id);
    }
    
    [Fact]
    public async Task FindAllCategoriesAsync_SortByNameRu_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _categoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CategorySortBy.NameRu);
        var categories = result.ToList();
        // Assert
        Assert.NotNull(categories);
        Assert.Equal(TestDataConstants.OverallCategoriesCount, categories.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, categories[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting4 , categories[1].Id);
        Assert.Equal(TestDataConstants.CategoryIdForUpdate, categories[2].Id);
        Assert.Equal(TestDataConstants.CategoryIdForDelete, categories[3].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting2, categories[4].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting5, categories[5].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, categories[6].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, categories[7].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting3, categories[8].Id);
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindCategoryByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var category = await _categoryRepository
            .FindByIdAsync(
                TestDataConstants.CategoryIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(category);
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, category.Id);
    }

    [Fact]
    public async Task FindCategoryByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 1435;
        // Act
        var category = await _categoryRepository
            .FindByIdAsync(
                wrongId, 
                CancellationToken.None);
        // Assert
        Assert.Null(category);
    }
    
    [Fact]
    public async Task GetCategoryByIdAsync_Success_Test() 
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var category = await _categoryRepository
            .GetByIdAsync(
                TestDataConstants.CategoryIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(category);
        Assert.Equal(TestDataConstants.CategoryIdForGetting1, category.Id);
    }
    
    [Fact]
    public async Task GetCategoryByIdAsync_FailOnWrongId_Test() 
    { 
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 143425;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _categoryRepository
                .GetByIdAsync(
                    id: wrongId,
                    CancellationToken.None));
    }
    #endregion
    #region By Key
    [Fact]
    public async Task FindCategoriesByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository
            .FindByNameAsync(
                TestDataConstants.CategoryNameForGetting7, 
                CancellationToken.None);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[1].Id);
    }

    [Fact]
    public async Task FindCategoryByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        var categories = await _categoryRepository
            .FindByNameAsync(
                name: wrongName, 
                CancellationToken.None);
        // Assert
        var result = categories.ToList();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository
            .GetByNameAsync(
                TestDataConstants.CategoryNameForGetting7, 
                CancellationToken.None);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[1].Id);
    }

    [Fact]
    public async Task GetCategoryByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _categoryRepository
                .GetByNameAsync(
                    name: wrongName, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task FindCategoryByNameAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository
            .FindByNameAsync(
                pageCount: 2,
                name: TestDataConstants.CategoryNameForGetting7, 
                cancellationToken: CancellationToken.None);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[1].Id);
    }
    
    [Fact]
    public async Task FindCategoryByNameAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository.FindByNameAsync(
            name: TestDataConstants.CategoryNameForGetting7,
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CategorySortBy.Key);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[1].Id);
    }

    [Fact]
    public async Task FindCategoryByNameAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository.FindByNameAsync(
            name: TestDataConstants.CategoryNameForGetting7,
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CategorySortBy.Key);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[1].Id);
    }

    [Fact]
    public async Task FindCategoryByNameAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository.FindByNameAsync(
            name: TestDataConstants.CategoryNameForGetting7,
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CategorySortBy.Id);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[1].Id);
    }

    [Fact]
    public async Task FindCategoryByNameAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository.FindByNameAsync(
            name: TestDataConstants.CategoryNameForGetting7,
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CategorySortBy.Id);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[1].Id);
    }
    
    [Fact]
    public async Task FindCategoryByNameRuAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _categoryRepository = new CategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var categories = await _categoryRepository
            .FindByNameAsync(
                TestDataConstants.CategoryNameRuForGetting7, 
                CancellationToken.None);
        var result = categories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CategoryIdForGetting5, result[0].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting6, result[1].Id);
        Assert.Equal(TestDataConstants.CategoryIdForGetting7, result[2].Id);
    }
    #endregion
}*/