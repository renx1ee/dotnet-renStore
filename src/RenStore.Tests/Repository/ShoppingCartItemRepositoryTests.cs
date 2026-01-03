/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class ShoppingCartItemRepositoryTests
{
    private ApplicationDbContext _context;
    private ShoppingCartItemRepository _shoppingCartItemRepository;

    #region Create Update Delete

    [Fact]
    public async Task CreateShoppingCartItemAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cartItem = new ShoppingCartItemEntity()
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Price = 3632,
            CartId = TestDataConstants.ShoppingCartIdForGetting1,
            ProductId = TestDataConstants.ProductIdForGetting7
        };
        // Act
        await _shoppingCartItemRepository.CreateAsync(cartItem, CancellationToken.None);
        // Assert
        var result = await _context.ShoppingCartItems
            .FirstOrDefaultAsync(c => 
                c.Id == cartItem.Id);
        Assert.NotNull(result);
        Assert.Equal(cartItem.Id, result.Id);
        Assert.Equal(cartItem.Quantity, result.Quantity);
        Assert.Equal(cartItem.Price, result.Price);
        Assert.Equal(cartItem.CartId, result.CartId);
        Assert.Equal(cartItem.ProductId, result.ProductId);
    }
    
    [Fact]
    public async Task CreateShoppingCartItemAsync_FailOnWrongCartId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cartItem = new ShoppingCartItemEntity()
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Price = 3632,
            CartId = Guid.NewGuid(),
            ProductId = TestDataConstants.ProductIdForGetting7
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () => 
            await _shoppingCartItemRepository
                .CreateAsync(
                    item: cartItem, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateShoppingCartItemAsync_FailOnWrongProductId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cartItem = new ShoppingCartItemEntity()
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Price = 3632,
            CartId = TestDataConstants.ShoppingCartIdForGetting2,
            ProductId = Guid.NewGuid()
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _shoppingCartItemRepository
                .UpdateAsync(
                    item: cartItem, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateShoppingCartItemAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int quantity = 34;
        var existsCartItem = await _context.ShoppingCartItems
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartItemIdForUpdate);
        Assert.NotNull(existsCartItem);
        existsCartItem.Quantity = quantity;
        // Act 
        await _shoppingCartItemRepository.UpdateAsync(existsCartItem, CancellationToken.None);
        // Assert 
        var result = await _context.ShoppingCartItems
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartItemIdForUpdate);
        Assert.NotNull(result);
        Assert.Equal(TestDataConstants.ShoppingCartItemIdForUpdate, result.Id);
        Assert.Equal(quantity, result.Quantity);
    }
    
    [Fact]
    public async Task UpdateShoppingCartItemAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cartItem = new ShoppingCartItemEntity()
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Price = 3632,
            CartId = TestDataConstants.ShoppingCartIdForGetting1,
            ProductId = TestDataConstants.ProductIdForGetting7
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _shoppingCartItemRepository
                .UpdateAsync(
                    item: cartItem, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateShoppingCartItemAsync_FailOnWrongCartId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cartItem = new ShoppingCartItemEntity()
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Price = 3632,
            CartId = Guid.NewGuid(),
            ProductId = TestDataConstants.ProductIdForGetting7
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _shoppingCartItemRepository
                .UpdateAsync(
                    item: cartItem, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateShoppingCartItemAsync_FailOnWrongProductId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var cartItem = new ShoppingCartItemEntity()
        {
            Id = Guid.NewGuid(),
            Quantity = 1,
            Price = 3632,
            CartId = TestDataConstants.ShoppingCartIdForGetting1,
            ProductId = Guid.NewGuid()
        };
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _shoppingCartItemRepository
                .UpdateAsync(
                    item: cartItem, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteShoppingCartItemAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var existsCartItem = await _context.ShoppingCartItems
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartItemIdForDelete);
        Assert.NotNull(existsCartItem);
        // Act
        await _shoppingCartItemRepository.DeleteAsync(TestDataConstants.ShoppingCartItemIdForDelete, CancellationToken.None);
        // Assert
        var result = await _context.ShoppingCartItems
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.ShoppingCartItemIdForDelete);
        Assert.Null(result);
    }
    
    [Fact]
    public async Task DeleteShoppingCartItemAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _shoppingCartItemRepository
                .DeleteAsync(
                    id: wrongId, 
                    CancellationToken.None));
    }
    
    #endregion
    #region All
    [Fact]
    public async Task FindAllCartItemsAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var carts = await _shoppingCartItemRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(carts);
        Assert.Equal(TestDataConstants.OverallShoppingCartItemsCount, carts.Count());
    }
    
    [Fact]
    public async Task FindAllCartItemsAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var carts = await _shoppingCartItemRepository
            .FindAllAsync(
                pageCount: 2,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(carts);
        Assert.Equal(2, carts.Count());
    }
    
    [Fact]
    public async Task FindAllCartItemsAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var carts = await _shoppingCartItemRepository
            .FindAllAsync(
                sortBy: ShoppingCartItemSortBy.Id,
                descending: false,
                cancellationToken: CancellationToken.None);
        
        var result = carts.ToList();
        // Assert
        Assert.NotNull(carts);
        Assert.Equal(TestDataConstants.OverallShoppingCartItemsCount, carts.Count());
    }
    
    [Fact]
    public async Task FindAllCartItemsAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var items = await _shoppingCartItemRepository
            .FindAllAsync(
                sortBy: ShoppingCartItemSortBy.Id,
                descending: true,
                cancellationToken: CancellationToken.None);
        
        var result = items.ToList();
        // Assert
        Assert.NotNull(items);
        Assert.Equal(TestDataConstants.OverallShoppingCartItemsCount, items.Count());
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindCartItemsByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var item = await _shoppingCartItemRepository
            .FindByIdAsync(
                TestDataConstants.ShoppingCartItemIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(item);
        Assert.Equal(TestDataConstants.ShoppingCartItemIdForGetting1, item.Id);
    }
    
    [Fact]
    public async Task FindCartItemsByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var item = await _shoppingCartItemRepository
            .FindByIdAsync(
                id: Guid.NewGuid(), 
                CancellationToken.None);
        // Assert
        Assert.Null(item);
    }
    
    [Fact]
    public async Task GetCartItemsByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var item = await _shoppingCartItemRepository
            .GetByIdAsync(
                TestDataConstants.ShoppingCartItemIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(item);
        Assert.Equal(TestDataConstants.ShoppingCartItemIdForGetting1, item.Id);
    }
    
    [Fact]
    public async Task GetCartItemsByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _shoppingCartItemRepository 
                .GetByIdAsync(
                    Guid.NewGuid(),
                    CancellationToken.None));
    }
    #endregion
    #region By Cart ID
    [Fact]
    public async Task FindCartItemsByCartIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var item = await _shoppingCartItemRepository
            .FindByCartIdAsync(
                TestDataConstants.ShoppingCartIdForGetting1, 
                CancellationToken.None);
        var result = item.ToList();
        // Assert
        Assert.NotNull(item);
        Assert.Equal(TestDataConstants.ShoppingCartIdForGetting1, result[0].CartId);
    }
    
    [Fact]
    public async Task FindCartItemsByCartIdAsync_FailOnWrongCartId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongCartId = Guid.NewGuid();
        // Act
        var result = await _shoppingCartItemRepository
            .FindByCartIdAsync(
                cartId: wrongCartId, 
                CancellationToken.None);
        // Assert
        Assert.Equal(result, new List<ShoppingCartItemEntity>());
    }
    
    [Fact]
    public async Task GetCartItemsByCartIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var item = await _shoppingCartItemRepository
            .GetByCartIdAsync(
                TestDataConstants.ShoppingCartIdForGetting1, 
                CancellationToken.None);
        var result = item.ToList();
        // Assert
        Assert.NotNull(item);
        Assert.Equal(TestDataConstants.ShoppingCartIdForGetting1, result[0].CartId);
    }
    
    [Fact]
    public async Task GetCartItemsByCartIdAsync_FailOnWrongUserId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _shoppingCartItemRepository = new ShoppingCartItemRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongCartId = Guid.NewGuid();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _shoppingCartItemRepository
                .GetByCartIdAsync(
                    cartId: wrongCartId,
                    CancellationToken.None));
    }
    #endregion
}*/