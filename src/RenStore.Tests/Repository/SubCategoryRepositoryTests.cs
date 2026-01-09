/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class SubCategoryRepositoryTests
{
    private ApplicationDbContext _context;
    private SubCategoryRepository _subCategoryRepository;

    #region Create Edit Delete

    [Fact]
    public async Task CreateSubCategoryAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int subCategoryId = 1535636;
        var subCategory = new SubCategoryEntity()
        {
            Id = subCategoryId,
            Name = "Test",
            NormalizedName = "TEST",
            NameRu = "Тест",
            NormalizedNameRu = "ТЕСТ",
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        };
        // Act
        await _subCategoryRepository.CreateAsync(subCategory, CancellationToken.None);
        // Assert
        var result = await _context.SubCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == subCategoryId);
        Assert.NotNull(result);
        Assert.Equal(subCategoryId, result.Id);
        Assert.Equal(subCategory.Name, result.Name);
        Assert.Equal(subCategory.NormalizedName, result.NormalizedName);
        Assert.Equal(subCategory.NameRu, result.NameRu);
        Assert.Equal(subCategory.NormalizedNameRu, result.NormalizedNameRu);
        Assert.Equal(subCategory.Description, result.Description);
        Assert.Equal(subCategory.IsActive, result.IsActive);
        Assert.Equal(subCategory.CreatedDate, result.CreatedDate);
    }
    
    [Fact]
    public async Task CreateSubCategoryAsync_FailOnDuplicateName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int categoryId = 15356;
        var category = new SubCategoryEntity()
        {
            Id = categoryId,
            Name = TestDataConstants.SubCategoryNameForGetting1,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting1.ToUpper(),
            NameRu = "Тест",
            NormalizedNameRu = "ТЕСТ",
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CategoryId = TestDataConstants.CategoryIdForGetting1
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _subCategoryRepository
                .CreateAsync(
                    subCategory: category, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateSubCategoryAsync_FailOnWrongCategoryId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int categoryId = 15356;
        var category = new SubCategoryEntity()
        {
            Id = categoryId,
            Name = TestDataConstants.SubCategoryNameForGetting1,
            NormalizedName = TestDataConstants.SubCategoryNameForGetting1.ToUpper(),
            NameRu = "Тест",
            NormalizedNameRu = "ТЕСТ",
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
            CategoryId = 634737
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _subCategoryRepository
                .CreateAsync(
                    subCategory: category, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateSubCategoryAsync_FailOnDuplicateNameRu_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int categoryId = 15356;
        var category = new SubCategoryEntity()
        {
            Id = categoryId,
            Name = "wfkwej",
            NormalizedName = "lkwwfwawf".ToUpper(),
            NameRu = TestDataConstants.SubCategoryNameRuForGetting1,
            NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting1.ToUpper(),
            Description = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedDate = DateTime.UtcNow,
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _subCategoryRepository
                .CreateAsync(
                    subCategory: category, 
                    CancellationToken.None));
        
    }
    
    [Fact]
    public async Task UpdateSubCategoryAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string subCategoryName = "Test";
        string subCategoryNameRu = "Test";
        string normalizedSubCategoryNameRu = "Test";
        string normalizedSubCategoryName = "Test";
        string subCategoryDescription = Guid.NewGuid().ToString();
        
        var existsSubCategory = await _context.SubCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SubCategoryIdForUpdate);
        
        existsSubCategory.Name = subCategoryName;
        existsSubCategory.NameRu = subCategoryNameRu;
        existsSubCategory.NormalizedName = normalizedSubCategoryName;
        existsSubCategory.NormalizedNameRu = normalizedSubCategoryNameRu;
        existsSubCategory.Description = subCategoryDescription;
        // Act 
        await _subCategoryRepository.UpdateAsync(existsSubCategory, CancellationToken.None);
        
        // Assert
        var result = await _context.SubCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SubCategoryIdForUpdate);
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, result.Id);
        Assert.Equal(subCategoryName, result.Name);
        Assert.Equal(subCategoryNameRu, result.NameRu);
        Assert.Equal(normalizedSubCategoryNameRu, result.NormalizedNameRu);
        Assert.Equal(normalizedSubCategoryName, result.NormalizedName);
        Assert.Equal(subCategoryDescription, result.Description);
    }
    
    [Fact]
    public async Task UpdateSubCategoryAsync_FailOnDuplicateName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string subCategoryDescription = Guid.NewGuid().ToString();
        
        var existsSubCategory = await _context.SubCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SubCategoryIdForUpdate);
        
        existsSubCategory.Name = TestDataConstants.SubCategoryNameForGetting1;
        existsSubCategory.NameRu = TestDataConstants.SubCategoryNameRuForGetting1;
        existsSubCategory.NormalizedName = TestDataConstants.SubCategoryNameForGetting1.ToUpper();
        existsSubCategory.NormalizedNameRu = TestDataConstants.SubCategoryNameRuForGetting1.ToUpper();
        existsSubCategory.Description = subCategoryDescription;
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _subCategoryRepository
                .UpdateAsync(
                    subCategory: existsSubCategory, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateSubCategoryAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 1535656;
        var subCategory = new SubCategoryEntity()
        {
            Id = wrongId,
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _subCategoryRepository
                .UpdateAsync(
                    subCategory: subCategory, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteSubCategoryAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var existsSubCategory = await _context.SubCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SubCategoryIdForDelete);
        Assert.NotNull(existsSubCategory);
        // Act
        await _subCategoryRepository.DeleteAsync(TestDataConstants.SubCategoryIdForDelete, CancellationToken.None);
        // Assert
        var result = await _context.SubCategories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SubCategoryIdForDelete);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteSubCategoryAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 15356;
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _subCategoryRepository
                .DeleteAsync(
                    id: wrongId, 
                    CancellationToken.None));
    }
    
    #endregion
    #region All
    [Fact]
    public async Task FindAllSubCategoriesAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var subCategories = await _subCategoryRepository.FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
    }

    [Fact]
    public async Task FindAllSubCategoriesAsync_WithCountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var subCategories = await _subCategoryRepository
            .FindAllAsync(
                cancellationToken: CancellationToken.None, 
                pageCount: 3);
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(3, subCategories.Count());
    }
    
    [Fact]
    public async Task FindAllSubCategoriesAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _subCategoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: SubCategorySortBy.Id);
        var subCategories = result.ToList();
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, subCategories[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete, subCategories[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, subCategories[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, subCategories[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, subCategories[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, subCategories[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, subCategories[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, subCategories[7].Id);
    }
    
    [Fact]
    public async Task FindAllSubCategoriesAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _subCategoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: SubCategorySortBy.Id);
        var subCategories = result.ToList();
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, subCategories[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, subCategories[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, subCategories[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, subCategories[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, subCategories[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, subCategories[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, subCategories[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete, subCategories[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, subCategories[8].Id);
    }
    
    [Fact]
    public async Task FindAllSubCategoriesAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _subCategoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: SubCategorySortBy.Name);
        var subCategories = result.ToList();
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, subCategories[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete , subCategories[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, subCategories[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, subCategories[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, subCategories[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, subCategories[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, subCategories[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, subCategories[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, subCategories[8].Id);
    }
    
    [Fact]
    public async Task FindAllSubCategoriesAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _subCategoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: SubCategorySortBy.Name);
        var subCategories = result.ToList();
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, subCategories[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6 , subCategories[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, subCategories[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, subCategories[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, subCategories[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, subCategories[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, subCategories[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete, subCategories[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, subCategories[8].Id);
    }
    
    [Fact]
    public async Task FindAllSubCategoriesAsync_SortByNameRu_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _subCategoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: SubCategorySortBy.NameRu);
        var subCategories = result.ToList();
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, subCategories[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete , subCategories[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, subCategories[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, subCategories[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, subCategories[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, subCategories[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, subCategories[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, subCategories[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, subCategories[8].Id);
    }
    
    [Fact]
    public async Task FindAllSubCategoriesAsync_SortByNameRu_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _subCategoryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: SubCategorySortBy.NameRu);
        var subCategories = result.ToList();
        // Assert
        Assert.NotNull(subCategories);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, subCategories[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6 , subCategories[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, subCategories[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, subCategories[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, subCategories[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, subCategories[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, subCategories[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete, subCategories[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, subCategories[8].Id);
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindSubCategoryByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var category = await _subCategoryRepository
            .FindByIdAsync(
                TestDataConstants.SubCategoryIdForGetting1,
                CancellationToken.None);
        // Assert
        Assert.NotNull(category);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, category.Id);
    }

    [Fact]
    public async Task FindSubCategoryByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 1435;
        // Act
        var category = await _subCategoryRepository
            .FindByIdAsync(
                wrongId,
                CancellationToken.None);
        // Assert
        Assert.Null(category);
    }

    [Fact]
    public async Task GetSubCategoryByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var category = await _subCategoryRepository
            .GetByIdAsync(
                TestDataConstants.SubCategoryIdForGetting1,
                CancellationToken.None);
        // Assert
        Assert.NotNull(category);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, category.Id);
    }

    [Fact]
    public async Task GetSubCategoryByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 143425;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _subCategoryRepository
                .GetByIdAsync(
                    id: wrongId,
                    CancellationToken.None));
    }
    #endregion
    #region By Name
    [Fact]
    public async Task FindSubCategoriesByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "Categ";
        // Act
        var subCategories = await _subCategoryRepository
            .FindByNameAsync(
                name: name,
                CancellationToken.None);
        var result = subCategories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, result.Count());
    }

    [Fact]
    public async Task FindSubCategoryByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        var subCategories = await _subCategoryRepository
            .FindByNameAsync(
                name: wrongName,
                CancellationToken.None);
        // Assert
        var result = subCategories.ToList();
        Assert.Empty(subCategories);
    }

    [Fact]
    public async Task GetSubCategoryByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "Categ";
        // Act
        var subCategores = await _subCategoryRepository
            .GetByNameAsync(
                name: name,
                CancellationToken.None);
        var result = subCategores.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, result.Count());
    }

    [Fact]
    public async Task GetSubCategoryByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _subCategoryRepository
                .GetByNameAsync(
                    name: wrongName,
                    CancellationToken.None));
    }

    [Fact]
    public async Task FindSubCategoryByNameAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "Categ";
        // Act
        var countries = await _subCategoryRepository
            .FindByNameAsync(
                pageCount: 2,
                name: name,
                cancellationToken: CancellationToken.None);
        var result = countries.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task FindSubCategoryByNameAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "Categ";
        // Act
        var subCategories = await _subCategoryRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None,
            descending: false,
            sortBy: SubCategorySortBy.Name);
        var result = subCategories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, result.Count());
        // TODO:
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, result[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete , result[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, result[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, result[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, result[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, result[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, result[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, result[8].Id);
    }

    [Fact]
    public async Task FindSubCategoryByNameAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "Categ";
        // Act
        var subCategories = await _subCategoryRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None,
            descending: true,
            sortBy: SubCategorySortBy.Name);
        var result = subCategories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, result.Count());
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, subCategories.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, result[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6 , result[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, result[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, result[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, result[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, result[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete, result[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, result[8].Id);
    }

    [Fact]
    public async Task FindSubCategoryByNameAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "Categ";
        // Act
        var subCategories = await _subCategoryRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None,
            descending: false,
            sortBy: SubCategorySortBy.Id);
        var result = subCategories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, result.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, result[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete, result[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, result[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, result[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, result[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, result[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, result[7].Id);
    }

    [Fact]
    public async Task FindSubCategoryByNameAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "Categ";
        // Act
        var subCategories = await _subCategoryRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None,
            descending: true,
            sortBy: SubCategorySortBy.Id);
        var result = subCategories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, result.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, result[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, result[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, result[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, result[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, result[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, result[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete, result[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, result[8].Id);
    }

    [Fact]
    public async Task FindSubCategoryByNameRuAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _subCategoryRepository = new SubCategoryRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "катег";
        // Act
        var subCategories = await _subCategoryRepository
            .FindByNameAsync(
                name: name,
                CancellationToken.None);
        var result = subCategories.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.OverallSubCategoriesCount, result.Count());
        Assert.Equal(TestDataConstants.SubCategoryIdForUpdate, result[0].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForDelete , result[1].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting1, result[2].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting2, result[3].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting4, result[5].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting5, result[6].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting6, result[7].Id);
        Assert.Equal(TestDataConstants.SubCategoryIdForGetting7, result[8].Id);
    }
    #endregion
    #region By Category ID
    
    #endregion 
}*/