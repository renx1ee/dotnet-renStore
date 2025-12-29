using Microsoft.EntityFrameworkCore;
using RenStore.Application.Common.Exceptions;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using Tests.Common;

namespace Tests.Repository;

public class SellerComplainRepositoryTests
{
    private ApplicationDbContext _context;
    private SellerComplainRepository _sellerComplainRepository;
    #region Create Update Delete
    
    [Fact]
    public async Task CreateSellerComplainAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = new SellerComplainEntity
        {
            Id = Guid.NewGuid(),
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedDate = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId = TestDataConstants.SellerIdForUpdate,
            UserId = TestDataConstants.UserIdForGettingSeller4
        };
        // Act
        await _sellerComplainRepository.CreateAsync(complain, CancellationToken.None);
        // Assert
        var result = await _context.SellerComplains
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == complain.Id);
        Assert.NotNull(result);
        Assert.Equal(complain.Id, result.Id);
        Assert.Equal(complain.Comment, result.Comment);
        Assert.Equal(complain.CreatedDate, result.CreatedDate);
        Assert.Equal(complain.Status, result.Status);
        Assert.Equal(complain.SellerId, result.SellerId);
        Assert.Equal(complain.UserId, result.UserId);
        Assert.Equal(complain.CustomReason, result.CustomReason);
    }
    
    [Fact]
    public async Task CreateSellerComplainAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = new SellerComplainEntity
        {
            Id = Guid.NewGuid(),
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedDate = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId = TestDataConstants.SellerIdForUpdate,
            UserId = Guid.NewGuid().ToString()
        };
        // Act & Assert
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(async () => 
            await _sellerComplainRepository
                .CreateAsync(
                    complain: complain, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateSellerComplainAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = await _context.SellerComplains
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SellerComplainIdForUpdate);
        Assert.NotNull(complain);
        complain.CustomReason = "fwawaffewwafw";
        complain.Comment = "totalPrice";
        complain.Status = SellerComplainStatus.InReview;
        // Act 
        await _sellerComplainRepository.UpdateAsync(complain, CancellationToken.None);
        // Assert
        var result = await _context.SellerComplains
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SellerComplainIdForUpdate);
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.SellerComplainIdForUpdate, result.Id);
        Assert.Equal(complain.CustomReason, result.CustomReason);
        Assert.Equal(complain.Comment, result.Comment);
        Assert.Equal(complain.Status, result.Status);
    }
    
    [Fact]
    public async Task UpdateSellerComplainAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var complain = new SellerComplainEntity
        {
            Id = Guid.NewGuid(),
            CustomReason = "fewfawfa",
            Comment = "wfwafaw",
            CreatedDate = DateTime.UtcNow,
            Status = SellerComplainStatus.New,
            SellerId = TestDataConstants.SellerIdForUpdate,
            UserId = Guid.NewGuid().ToString()
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _sellerComplainRepository
                .UpdateAsync(
                    complain, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteSellerComplainAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var existsCartItem = await _context.SellerComplains
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SellerComplainIdForDelete);
        Assert.NotNull(existsCartItem);
        // Act
        await _sellerComplainRepository.DeleteAsync(TestDataConstants.SellerComplainIdForDelete, CancellationToken.None);
        // Assert
        var result = await _context.SellerComplains
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.SellerComplainIdForDelete);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteSellerComplainAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _sellerComplainRepository
                .DeleteAsync(
                    id: wrongId, 
                    CancellationToken.None));
    }
    
    #endregion
    #region All
    [Fact]
    public async Task FindAllSellerComplainsAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _sellerComplainRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(TestDataConstants.OverallSellerComplainsCount, complains.Count());
    }
    
    [Fact]
    public async Task FindAllSellerComplainsAsync_CountLimit_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _sellerComplainRepository
            .FindAllAsync(
                pageCount: 2,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(2, complains.Count());
    }
    
    [Fact]
    public async Task FindAllSellerComplainsAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _sellerComplainRepository
            .FindAllAsync(
                sortBy: SellerComplainSortBy.Id,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = complains.ToList();
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(TestDataConstants.OverallSellerComplainsCount, complains.Count());
    }
    
    [Fact]
    public async Task FindAllSellerComplainsAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complains = await _sellerComplainRepository
            .FindAllAsync(
                sortBy: SellerComplainSortBy.Id,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = complains.ToList();
        // Assert
        Assert.NotNull(complains);
        Assert.Equal(TestDataConstants.OverallSellerComplainsCount, complains.Count());
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindSellerComplainByIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _sellerComplainRepository
            .FindByIdAsync(
                TestDataConstants.SellerComplainIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.SellerComplainIdForGetting1, complain.Id);
    }
    
    [Fact]
    public async Task FindSellerComplainByIdAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _sellerComplainRepository
            .FindByIdAsync(
                id: Guid.NewGuid(), 
                CancellationToken.None);
        // Assert
        Assert.Null(complain);
    }
    
    [Fact]
    public async Task GetSellerComplainByIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _sellerComplainRepository
            .GetByIdAsync(
                TestDataConstants.SellerComplainIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.SellerComplainIdForGetting1, complain.Id);
    }
    
    [Fact]
    public async Task GetSellerComplainByIdAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _sellerComplainRepository 
                .GetByIdAsync(
                    Guid.NewGuid(),
                    CancellationToken.None));
    }
    #endregion
    #region By User ID
    [Fact]
    public async Task FindSellerComplainsByUserIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _sellerComplainRepository
            .FindByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        var result = complain.ToList();
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.UserIdForGettingSeller1, result[0].UserId);
    }
    
    [Fact]
    public async Task FindSellerComplainsByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        var result = await _sellerComplainRepository
            .FindByUserIdAsync(
                userId: wrongUserId, 
                CancellationToken.None);
        // Assert
        Assert.Equal(result, new List<SellerComplainEntity>());
    }
    
    [Fact]
    public async Task GetSellerComplainsByUserIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
       _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var complain = await _sellerComplainRepository
            .GetByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        var result = complain.ToList();
        // Assert
        Assert.NotNull(complain);
        Assert.Equal(TestDataConstants.UserIdForGettingSeller1, result[0].UserId);
    }
    
    [Fact]
    public async Task GetSellerComplainsByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _sellerComplainRepository = new SellerComplainRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _sellerComplainRepository
                .GetByUserIdAsync(
                    userId: wrongUserId,
                    CancellationToken.None));
    }
    #endregion
}