using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using Tests.Common;

namespace Tests.Repository;

public class SellerRepositoryTests : IDisposable
{
    private ApplicationDbContext _context;
    private SellerRepository _sellerRepository;
    #region Create Update Delete
    [Fact]
    public async Task CreateSellerAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arranges
        var user = await _context.AspNetUsers
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.Id == TestDataConstants.UserIdForCreateSeller);
        
        var seller = new SellerEntity
        {
            Id = TestDataConstants.SellerIdForCreate,
            Name = "Sample Name for Update",
            Description = "Sample Description for Update",
            NormalizedName = Guid.NewGuid().ToString().ToUpper(),
            CreatedDate = DateTime.UtcNow,
            IsBlocked = false,
            ApplicationUserId = TestDataConstants.UserIdForCreateSeller
        };
        // Act
        var result = await _sellerRepository.CreateAsync(seller, CancellationToken.None);
        // Assert
        var sellerExists = await _context.Sellers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.SellerIdForCreate);
        
        Assert.NotNull(sellerExists);
        Assert.Equal(TestDataConstants.SellerIdForCreate, sellerExists.Id);
        Assert.Equal(seller.Name, sellerExists.Name);
        Assert.Equal(seller.Description, sellerExists.Description);
        Assert.Equal(seller.NormalizedName, sellerExists.NormalizedName);
        Assert.Equal(seller.CreatedDate, sellerExists.CreatedDate);
        Assert.Equal(seller.IsBlocked, sellerExists.IsBlocked);
    }
    
    [Fact]
    
    public async Task CreateSellerAsync_FailOnEmpty_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
            await _sellerRepository
                .CreateAsync(new SellerEntity(),
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateSellerAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string updatedSellerName = nameof(updatedSellerName);
        string updatedSellerNormalizedName = nameof(updatedSellerNormalizedName);
        string updatedSellerDescription = nameof(updatedSellerDescription);
        bool updatedSellerIsBlocked = true;
        var sellerExists = await _context.Sellers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.SellerIdForUpdate);
        
        if (sellerExists is null)
            Assert.Fail();
        // Act
        sellerExists.Name = updatedSellerName;
        sellerExists.NormalizedName = updatedSellerNormalizedName;
        sellerExists.Description = updatedSellerDescription;
        sellerExists.IsBlocked = updatedSellerIsBlocked;
        
        await _sellerRepository.UpdateAsync(sellerExists, CancellationToken.None);
        // Assert
        var sellerResult = await _context.Sellers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.SellerIdForUpdate);
        Assert.NotNull(sellerResult);
        Assert.Equal(updatedSellerName, sellerResult.Name);
        Assert.Equal(updatedSellerNormalizedName, sellerResult.NormalizedName);
        Assert.Equal(updatedSellerDescription, sellerResult.Description);
        Assert.Equal(updatedSellerIsBlocked, sellerResult.IsBlocked);
    }
    
    [Fact]
    public async Task UpdateSellerAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        long wrongSelleId = 2357894329785;
        string updatedSellerName = nameof(updatedSellerName);
        string updatedSellerNormalizedName = nameof(updatedSellerNormalizedName);
        string updatedSellerDescription = nameof(updatedSellerDescription);
        bool updatedSellerIsBlocked = true;
        // Act
        var seller = new SellerEntity()
        {
            Id = wrongSelleId,
            Name = updatedSellerName,
            NormalizedName = updatedSellerNormalizedName,
            Description = updatedSellerDescription,
            IsBlocked = updatedSellerIsBlocked
        };
        // Assert
        var sellerResult = 
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _sellerRepository.UpdateAsync(
                seller, 
                CancellationToken.None));
    }

    [Fact]
    public async Task DeleteSellerAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Assert
        var sellerExists = await _context.Sellers
            .AsNoTracking()
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.SellerIdForDelete);
        Assert.NotNull(sellerExists);
        // Act
        await _sellerRepository.DeleteAsync(
            TestDataConstants.SellerIdForDelete, 
            CancellationToken.None);
        // Assert
        var sellerResult = await _context.Sellers
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.SellerIdForDelete);
        
        Assert.Null(sellerResult);
    }
    
    [Fact]
    public async Task DeleteSellerAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        long wrongId = 3242367;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _sellerRepository.DeleteAsync(
                id: wrongId,
                CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllSellersAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(6, sellers.Count());
    }
    
    [Fact]
    public async Task FindAllSellersAsync_CountLimit_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(
                pageCount: 2,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(2, sellers.Count());
    }
    
    [Fact]
    public async Task FindAllSellersAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(
                sortBy: SellerSortBy.Name,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(6, sellers.Count());
        Assert.Equal(TestDataConstants.SellerIdForUpdate, result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForDelete, result[1].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[2].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting2, result[3].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[5].Id);
        
    }
    
    [Fact]
    public async Task FindAllSellersAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(
                sortBy: SellerSortBy.Name,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(6, sellers.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[3].Id);
        Assert.Equal(TestDataConstants.SellerIdForDelete, result[4].Id);
        Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);
    }
    
    [Fact]
    public async Task FindAllSellersAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(
                sortBy: SellerSortBy.Id,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(6, sellers.Count());
        Assert.Equal(TestDataConstants.SellerIdForUpdate , result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[1].Id);
        Assert.Equal(TestDataConstants.SellerIdForDelete, result[2].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting2, result[3].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[5].Id);
    }
    
    [Fact]
    public async Task FindAllSellersAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(
                sortBy: SellerSortBy.Id,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(6, sellers.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
        Assert.Equal(TestDataConstants.SellerIdForDelete, result[3].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[4].Id);
        Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);
    }
    
    [Fact]
    public async Task FindAllSellersAsync_SortByCreatedDate_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
    }
    
    [Fact]
    public async Task FindAllSellersAsync_SortByCreatedDate_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
    }
    
    [Fact]
    public async Task FindAllSellersAsync_WithIsBlockedTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(
                isBlocked: true,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(2, sellers.Count());
    }
    
    [Fact]
    public async Task FindAllSellersAsync_WithIsBlockedFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindAllAsync(
                isBlocked: false,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(4, sellers.Count());
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindSellerByIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var seller = await _sellerRepository
            .FindByIdAsync(
                TestDataConstants.SellerIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(seller);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, seller.Id);
    }
    
    [Fact]
    public async Task FindSellerByIdAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        long wrongId = 32445362367;
        // Act
        var seller = await _sellerRepository
            .FindByIdAsync(
                id: wrongId, 
                CancellationToken.None);
        // Assert
        Assert.Null(seller);
    }
    
    [Fact]
    public async Task GetSellerByIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var seller = await _sellerRepository
            .GetByIdAsync(
                TestDataConstants.SellerIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(seller);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, seller.Id);
    }
    
    [Fact]
    public async Task GetSellerByIdAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        long wrongId = 324453653;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _sellerRepository 
                .GetByIdAsync(
                    wrongId,
                    CancellationToken.None));
    }
    #endregion
    #region By Name
    // TODO:
    [Fact]
    public async Task FindSellerByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(
                TestDataConstants.SellerNameForGetting1, 
                CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[0].Id);
    }
    
    [Fact]
    public async Task FindSellerByNameAsync_FailOnWrongName_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        var seller = await _sellerRepository
            .FindByNameAsync(
                name: wrongName, 
                CancellationToken.None);
        // Assert
        Assert.Empty(seller);
    }
    
    [Fact]
    public async Task GetSellerByNameAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .GetByNameAsync(
                TestDataConstants.SellerNameForGetting1, 
                CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[0].Id);
    }
    
    [Fact]
    public async Task GetSellerByNameAsync_FailOnWrongName_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _sellerRepository
                .GetByUserIdAsync(
                    userId: wrongUserId,
                    CancellationToken.None));
    }
    //TODO: зачем повторяется
    [Fact]
    public async Task FindByNameSellersAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(TestDataConstants.SellerNameForGetting4,CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(2, sellers.Count());
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_CountLimit_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(TestDataConstants.SellerNameForGetting4,
                pageCount: 2,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(2, sellers.Count());
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(TestDataConstants.SellerNameForGetting4,
                sortBy: SellerSortBy.Name,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(2, sellers.Count());
        
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[1].Id);
        
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(TestDataConstants.SellerNameForGetting4,
                sortBy: SellerSortBy.Name,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(sellers);
        Assert.Equal(2, sellers.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[1].Id);
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(TestDataConstants.SellerNameForGetting4,
                sortBy: SellerSortBy.Id,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[1].Id);
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(TestDataConstants.SellerNameForGetting4,
                sortBy: SellerSortBy.Id,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[1].Id);
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_SortByCreatedDate_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(
                name: TestDataConstants.SellerNameForGetting4, 
                cancellationToken: CancellationToken.None,
                sortBy: SellerSortBy.CreatedDate,
                descending: false);
        
        var result = sellers.ToList();
        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[1].Id);
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_SortByCreatedDate_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(name: TestDataConstants.SellerNameForGetting4, 
                cancellationToken: CancellationToken.None,
                sortBy: SellerSortBy.CreatedDate,
                descending: true);
        
        var result = sellers.ToList();
        // Assert
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[1].Id);
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_WithIsBlockedTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(
                TestDataConstants.SellerNameForGetting4,
                isBlocked: true,
                cancellationToken: CancellationToken.None);
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[0].Id);
    }
    
    [Fact]
    public async Task FindByNameSellersAsync_WithIsBlockedFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var sellers = await _sellerRepository
            .FindByNameAsync(TestDataConstants.SellerNameForGetting4,
                isBlocked: false,
                cancellationToken: CancellationToken.None);
        var result = sellers.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Count());
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[0].Id);
    }
    
    #endregion
    #region By User ID
    [Fact]
    public async Task FindSellerByUserIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var seller = await _sellerRepository
            .FindByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(seller);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, seller.Id);
    }
    
    [Fact]
    public async Task FindSellerByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        var result = await _sellerRepository
            .FindByUserIdAsync(
                userId: wrongUserId, 
                CancellationToken.None);
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetSellerByUserIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var seller = await _sellerRepository
            .GetByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(seller);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, seller.Id);
    }
    
    [Fact]
    public async Task GetSellerByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerRepository = new SellerRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _sellerRepository
                .GetByUserIdAsync(
                    userId: wrongUserId,
                    CancellationToken.None));
    }
    #endregion
    public void Dispose()
    {
        /*_context.Dispose();
        _context.Database.EnsureDeleted();*/
    }
}