/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class /*#1#ShoppingCartRepositoryTests
{
    private ApplicationDbContext _context;
    private ShoppingCartRepository _shoppingCartRepository;

    #region Create Edit Delete

    [Fact]
    public async Task CreateShoppingCartAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cart = new ShoppingCartEntity()
        {
            Id = Guid.NewGuid(),
            TotalPrice = 21456,
            OccuredAt = DateTime.UtcNow,
            UserId = TestDataConstants.UserIdForGettingSeller4
        };
        // Act
        await _shoppingCartRepository.CreateAsync(cart, CancellationToken.None);
        // Assert
        var result = await _context.ShoppingCarts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == cart.Id);
        Assert.NotNull(result);
        Assert.Equal(cart.Id, result.Id);
        Assert.Equal(cart.TotalPrice, result.TotalPrice);
        Assert.Equal(cart.OccuredAt, result.OccuredAt);
        Assert.Equal(cart.UserId, result.UserId);
    }
    
    [Fact]
    public async Task CreateShoppingCartAsync_FailOnWrongUserId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cart = new ShoppingCartEntity()
        {
            Id = Guid.NewGuid(),
            TotalPrice = 21456,
            OccuredAt = DateTime.UtcNow,
            UserId = Guid.NewGuid().ToString()
        };
        // Act & Assert
        await Assert.ThrowsAsync<Microsoft.EntityFrameworkCore.DbUpdateException>(async () => 
            await _shoppingCartRepository
                .CreateAsync(
                    cart: cart, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateShoppingCartAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int totalPrice = 1535;
        var existsCart = await _context.ShoppingCarts
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartIdForUpdate);
        existsCart.TotalPrice = totalPrice;
        // Act 
        await _shoppingCartRepository.UpdateAsync(existsCart, CancellationToken.None);
        // Assert
        var result = await _context.ShoppingCarts
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartIdForUpdate);
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.ShoppingCartIdForUpdate, result.Id);
        Assert.Equal(totalPrice, result.TotalPrice);
    }
    
    [Fact]
    public async Task UpdateShoppingCartAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
       _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cart = new ShoppingCartEntity()
        {
            Id = Guid.NewGuid(),
            TotalPrice = 21456,
            OccuredAt = DateTime.UtcNow,
            UserId = TestDataConstants.UserIdForGettingSeller5
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _shoppingCartRepository
                .UpdateAsync(
                    cart: cart, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteShoppingCartAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var existsCartItem = await _context.ShoppingCarts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartIdForDelete);
        Assert.NotNull(existsCartItem);
        // Act
        await _shoppingCartRepository.DeleteAsync(TestDataConstants.ShoppingCartIdForDelete, CancellationToken.None);
        // Assert
        var result = await _context.ShoppingCarts
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartIdForDelete);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteShoppingCartAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _shoppingCartRepository
                .DeleteAsync(
                    id: wrongId, 
                    CancellationToken.None));
    }
    
    #endregion
    #region All
    [Fact]
    public async Task FindAllShoppingCartsAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var carts = await _shoppingCartRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(carts);
        Assert.Equal(TestDataConstants.OverallShoppingCartsCount, carts.Count());
    }
    
    [Fact]
    public async Task FindAllShoppingCartsAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var carts = await _shoppingCartRepository
            .FindAllAsync(
                pageCount: 2,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(carts);
        Assert.Equal(2, carts.Count());
    }
    
    [Fact]
    public async Task FindAllShoppingCartsAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var carts = await _shoppingCartRepository
            .FindAllAsync(
                sortBy: ShoppingCartSortBy.Id,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = carts.ToList();
        // Assert
        Assert.NotNull(carts);
        Assert.Equal(TestDataConstants.OverallShoppingCartsCount, carts.Count());
    }
    
    [Fact]
    public async Task FindAllShoppingCartsAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var carts = await _shoppingCartRepository
            .FindAllAsync(
                sortBy: ShoppingCartSortBy.Id,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = carts.ToList();
        // Assert
        Assert.NotNull(carts);
        Assert.Equal(TestDataConstants.OverallShoppingCartsCount, carts.Count());
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindCartByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var cart = await _shoppingCartRepository
            .FindByIdAsync(
                TestDataConstants.ShoppingCartIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(cart);
        Assert.Equal(TestDataConstants.ShoppingCartIdForGetting1, cart.Id);
    }
    
    [Fact]
    public async Task FindCartByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var cart = await _shoppingCartRepository
            .FindByIdAsync(
                id: Guid.NewGuid(), 
                CancellationToken.None);
        // Assert
        Assert.Null(cart);
    }
    
    [Fact]
    public async Task GetCartByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var cart = await _shoppingCartRepository
            .GetByIdAsync(
                TestDataConstants.ShoppingCartIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(cart);
        Assert.Equal(TestDataConstants.ShoppingCartIdForGetting1, cart.Id);
    }
    
    [Fact]
    public async Task GetCartByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _shoppingCartRepository 
                .GetByIdAsync(
                    Guid.NewGuid(),
                    CancellationToken.None));
    }
    #endregion
    #region By User ID
    [Fact]
    public async Task FindCartByUserIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var cart = await _shoppingCartRepository
            .FindByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(cart);
        Assert.Equal(TestDataConstants.UserIdForGettingSeller1, cart.UserId);
    }
    
    [Fact]
    public async Task FindCartByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        var result = await _shoppingCartRepository
            .FindByUserIdAsync(
                userId: wrongUserId, 
                CancellationToken.None);
        // Assert
        Assert.Null(result);
    }
    
    [Fact]
    public async Task GetCartByUserIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var seller = await _shoppingCartRepository
            .GetByUserIdAsync(
                TestDataConstants.UserIdForGettingSeller1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(seller);
        Assert.Equal(TestDataConstants.UserIdForGettingSeller1, seller.UserId);
    }
    
    [Fact]
    public async Task GetCartByUserIdAsync_FailOnWrongUserId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartRepository = new ShoppingCartRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongUserId = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _shoppingCartRepository
                .GetByUserIdAsync(
                    userId: wrongUserId,
                    CancellationToken.None));
    }
    #endregion
}*/