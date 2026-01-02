using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class ColorRepositoryTests : IDisposable
{
    private ApplicationDbContext _context;
    private ColorRepository _colorRepository;
    
    [Fact]
    public async Task CreateColorAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int colorId = 84587;
        var color = new ColorEntity()
        {
            Id = colorId,
            Name = TestDataConstants.ColorNameForCreate,
            NormalizedName = TestDataConstants.ColorNameForCreate.ToUpper(),
            NameRu = Guid.NewGuid().ToString(),
            ColorCode = "#123",
            Description = Guid.NewGuid().ToString()
        };
        // Act
        var result = await _colorRepository.CreateAsync(color, CancellationToken.None);
        // Assert
        Assert.Equal(colorId, result);
        var savedColor = await _context.Colors
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == TestDataConstants.ColorNameForCreate);
        
        Assert.NotNull(savedColor);
        Assert.Equal(color.Name, savedColor.Name);
        Assert.Equal(color.NormalizedName, savedColor.NormalizedName);
        Assert.Equal(color.NameRu, savedColor.NameRu);
        Assert.Equal(color.ColorCode, savedColor.ColorCode);
        Assert.Equal(color.Description, savedColor.Description);
    }
    // TODO: сделать duplicate exception
    [Fact]
    public async Task CreateColorAsync_FailOnDuplicateName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int colorId = 4345;
        var color = new ColorEntity()
        {
            Id = colorId,
            Name = TestDataConstants.ColorNameForCreate,
            NormalizedName = TestDataConstants.ColorNameForCreate.ToUpper(),
            NameRu = "белый",
            ColorCode = "#FFF",
            Description = "Test Description"
        };
        var duplicateColor = new ColorEntity()
        {
            Id = colorId + 1,
            Name = TestDataConstants.ColorNameForCreate,
            NormalizedName = TestDataConstants.ColorNameForCreate.ToUpper(),
            NameRu = "белый",
            ColorCode = "#FFF",
            Description = "Test Description"
        };
        // Act 
        await _colorRepository.CreateAsync(color, CancellationToken.None);
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
            await _colorRepository.CreateAsync(
                color: duplicateColor, 
                CancellationToken.None));
        
        var duplicateSavedColor = await _context.Colors
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == colorId + 1);
        
        Assert.Null(duplicateSavedColor);
    }
    
    [Fact]
    public async Task UpdateColorAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string updatedName = Guid.NewGuid().ToString();
        string updatedNormalizedName = updatedName.ToUpper();
        string description = Guid.NewGuid().ToString();
        var color = await _context.Colors.FirstOrDefaultAsync(c => 
            c.Id == TestDataConstants.ColorIdForUpdate);
        if (color is null)
            return;
        // Act
        color.Name = updatedName;
        color.NormalizedName = updatedNormalizedName;
        color.Description = description;
        await _colorRepository.UpdateAsync(color, CancellationToken.None);
        // Assert
        var updatedColor = await _context.Colors.FirstOrDefaultAsync(c => 
            c.Id == TestDataConstants.ColorIdForUpdate);
        Assert.NotNull(updatedColor);
        Assert.Equal(updatedName, updatedColor.Name);
        Assert.Equal(updatedNormalizedName, updatedColor.NormalizedName);
        Assert.Equal(description, updatedColor.Description);
    }
    
    [Fact]
    public async Task UpdateColorAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string updatedName = Guid.NewGuid().ToString();
        string updatedNormalizedName = updatedName.ToUpper();
        string description = Guid.NewGuid().ToString();
        // Act
        var color = new ColorEntity()
        {
            Id = 63263774,
            Name = updatedName,
            NormalizedName = updatedNormalizedName,
            Description = description
        };
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _colorRepository.UpdateAsync(
                color: color, 
                CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteColorAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        await _colorRepository.DeleteAsync(
            TestDataConstants.ColorIdForDelete, 
            CancellationToken.None);
        // Assert
        var color = await _context.Colors
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ColorIdForDelete);
        
        Assert.Null(color);
    }
    
    [Fact]
    public async Task DeleteColorAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _colorRepository.DeleteAsync(
                id: 254363, 
                CancellationToken.None));
    }
    
    [Fact]
    public async Task FindAllColorsAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _colorRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(8, result.Count());
    }
    
    [Fact]
    public async Task FindAllColorsAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _colorRepository
            .FindAllAsync(
                pageCount: 5, 
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(5, result.Count());
    }
    
    [Fact]
    public async Task FindAllColorsAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var colors = await _colorRepository
            .FindAllAsync(
                sortBy: ColorSortBy.NormalizedName,
                descending: false,
                cancellationToken: CancellationToken.None);
        // Assert
        var result = colors.ToList();
        Assert.NotNull(result);
        Assert.Equal(8, result.Count());
        
        Assert.Equal(TestDataConstants.ColorNameForUpdate, result[0].Name);
        Assert.Equal(TestDataConstants.ColorNameForGetting1, result[1].Name);
        Assert.Equal(TestDataConstants.ColorNameForGetting6, result[2].Name); 
        Assert.Equal(TestDataConstants.ColorNameForGetting5, result[3].Name);
        Assert.Equal(TestDataConstants.ColorNameForDelete, result[4].Name); 
        Assert.Equal(TestDataConstants.ColorNameForGetting2, result[5].Name); 
        Assert.Equal(TestDataConstants.ColorNameForGetting4, result[6].Name);
        Assert.Equal(TestDataConstants.ColorNameForGetting3, result[7].Name);
    }
    
    [Fact]
    public async Task FindAllColorsAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var colors = await _colorRepository
            .FindAllAsync(
                sortBy: ColorSortBy.NormalizedName,
                descending: true,
                cancellationToken: CancellationToken.None);
        // Assert
        var result = colors.ToList();
        Assert.NotNull(result);
        Assert.Equal(8, result.Count());
        Assert.Equal(TestDataConstants.ColorNameForGetting3, result[0].Name);
        Assert.Equal(TestDataConstants.ColorNameForGetting4, result[1].Name);
        Assert.Equal(TestDataConstants.ColorNameForGetting2, result[2].Name); 
        Assert.Equal(TestDataConstants.ColorNameForDelete, result[3].Name);
        Assert.Equal(TestDataConstants.ColorNameForGetting5, result[4].Name); 
        Assert.Equal(TestDataConstants.ColorNameForGetting6, result[5].Name); 
        Assert.Equal(TestDataConstants.ColorNameForGetting1, result[6].Name);
        Assert.Equal(TestDataConstants.ColorNameForUpdate, result[7].Name);
    }
    
    [Fact]
    public async Task FindAllColorsAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var colors = await _colorRepository
            .FindAllAsync(
                sortBy: ColorSortBy.Id,
                descending: false,
                cancellationToken: CancellationToken.None);
        // Assert
        var result = colors.ToList();
        Assert.NotNull(result);
        Assert.Equal(8, result.Count());
        Assert.Equal(TestDataConstants.ColorIdForUpdate, result[0].Id); // 1 1
        Assert.Equal(TestDataConstants.ColorIdForDelete, result[1].Id); // 2 3
        Assert.Equal(TestDataConstants.ColorIdForGetting1, result[2].Id); // 3 6
        Assert.Equal(TestDataConstants.ColorIdForGetting2, result[3].Id); // 4 7
        Assert.Equal(TestDataConstants.ColorIdForGetting3, result[4].Id); // 5 2
        Assert.Equal(TestDataConstants.ColorIdForGetting4, result[5].Id); // 6 4
        Assert.Equal(TestDataConstants.ColorIdForGetting5, result[6].Id); // 7 5
        Assert.Equal(TestDataConstants.ColorIdForGetting6, result[7].Id);
    }
    
    [Fact]
    public async Task FindAllColorsAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var colors = await _colorRepository
            .FindAllAsync(
                sortBy: ColorSortBy.Id,
                descending: true,
                cancellationToken: CancellationToken.None);
        // Assert
        var result = colors.ToList();
        Assert.NotNull(result);
        Assert.Equal(8, result.Count());
        Assert.Equal(TestDataConstants.ColorIdForGetting6, result[0].Id);
        Assert.Equal(TestDataConstants.ColorIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.ColorIdForGetting4 , result[2].Id);
        Assert.Equal(TestDataConstants.ColorIdForGetting3, result[3].Id);
        Assert.Equal(TestDataConstants.ColorIdForGetting2, result[4].Id);
        Assert.Equal(TestDataConstants.ColorIdForGetting1, result[5].Id);
        Assert.Equal(TestDataConstants.ColorIdForDelete, result[6].Id);
        Assert.Equal(TestDataConstants.ColorIdForUpdate, result[7].Id);
    }
    
    [Fact]
    public async Task FindColorByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _colorRepository
            .FindByIdAsync(
                TestDataConstants.ColorIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.ColorIdForGetting1, result.Id);
    }
    
    [Fact]
    public async Task FindColorByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _colorRepository
            .FindByIdAsync(
                id: 427324242, 
                CancellationToken.None);
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetColorByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _colorRepository
            .GetByIdAsync(
                id: TestDataConstants.ColorIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.ColorIdForGetting1, result.Id);
    }
    
    [Fact]
    public async Task GetColorByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int colorId = 5353336;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _colorRepository.GetByIdAsync(
                id: colorId, 
                CancellationToken.None));
    }
    
    [Fact]
    public async Task FindColorByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var colors = await _colorRepository
            .FindByNameAsync(
                name: TestDataConstants.ColorNameForGetting1, 
                CancellationToken.None);
        
        var result = colors.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.ColorIdForGetting1, result[0].Id);
    }
    
    [Fact]
    public async Task FindColorByNameAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var colors = await _colorRepository
            .FindByNameAsync(
                pageCount: 2,
                name: TestDataConstants.ColorNameForGetting5, 
                cancellationToken: CancellationToken.None);
        
        var result = colors.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task FindColorByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string colorName = Guid.NewGuid().ToString();
        // Act
        var result = await _colorRepository
            .FindByNameAsync(colorName, CancellationToken.None);
        // Assert
        Assert.Equal([], result);
    }
    
    [Fact]
    public async Task FindColorByNameAsync_FailOnNullName_Test()
    { 
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async() =>
            await _colorRepository
                .FindByNameAsync(
                    string.Empty,
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task FindColorByNameAsync_FailOnEmptyName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async() =>
            await _colorRepository
                .FindByNameAsync(
                    string.Empty,
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task GetColorByNameAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var colors = await _colorRepository
            .GetByNameAsync(
                name: TestDataConstants.ColorNameForGetting1,
                CancellationToken.None);
        
        var result = colors.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.ColorNameForGetting1, result[0].Name);
    }
    
    [Fact]
    public async Task GetColorByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _colorRepository
                .GetByNameAsync(
                    name: Guid.NewGuid().ToString(), 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task GetColorByNameAsync_FailOnNullName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async() =>
            await _colorRepository
                .FindByNameAsync(
                    string.Empty,
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task GetColorByNameAsync_FailOnEmptyName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _colorRepository = new ColorRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async() =>
            await _colorRepository
                .FindByNameAsync(
                    string.Empty,
                    CancellationToken.None));
    }
    
    public void Dispose()
    {
        /*_context.Database.EnsureDeleted();
        _context.Dispose();*/
    }
}