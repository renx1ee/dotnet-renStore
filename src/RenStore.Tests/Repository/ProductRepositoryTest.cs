/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;
using Xunit.Abstractions;

namespace Tests.Repository;

public class ProductRepositoryTest
{
    private readonly ITestOutputHelper testOutputHelper;
    private ApplicationDbContext _context;
    private ProductRepository _productRepository;

    public ProductRepositoryTest(ITestOutputHelper testOutputHelper)
    {
        this.testOutputHelper = testOutputHelper;
    }

    #region Create Edit Delete
    [Fact]
    public async Task CreateProductAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arranges
        var id = Guid.NewGuid();

        var product = new Product()
        {
            Id = id,
            IsBlocked = false,
            OverallRating = 5,
            SellerId = TestDataConstants.SellerIdForGetting4,
            CategoryId = TestDataConstants.CategoryIdForGetting5
        };
        
        // Act
        var result = await _productRepository.CreateAsync(product, CancellationToken.None);
        // Assert
        Assert.Equal(id, result);
        var productExists = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => 
                p.Id == id);
        
        Assert.NotNull(productExists);
        Assert.Equal(id, productExists.Id);
        Assert.Equal(product.IsBlocked, productExists.IsBlocked);
        Assert.Equal(product.OverallRating, productExists.OverallRating);
        Assert.Equal(product.SellerId, productExists.SellerId);
        Assert.Equal(product.CategoryId, productExists.CategoryId);
    }
    
    [Fact]
    public async Task CreateProductAsync_FailOnEmpty_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var product = new Product();
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async() => 
            await _productRepository
                .CreateAsync(
                    product, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateProductAsync_FailOnWrongProductId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            IsBlocked = false,
            OverallRating = 5,
            SellerId = 67832,
            CategoryId = TestDataConstants.CategoryIdForGetting5
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async() => 
            await _productRepository
                .CreateAsync(
                    product, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task CreateProductAsync_FailOnWrongCategoryId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var product = new Product()
        {
            Id = Guid.NewGuid(),
            IsBlocked = false,
            OverallRating = 5,
            SellerId = TestDataConstants.SellerIdForGetting4,
            CategoryId = 8525
        };
        // Act & Assert
        await Assert.ThrowsAsync<DbUpdateException>(async() => 
            await _productRepository
                .CreateAsync(
                    product, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateProductAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var productExists = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => 
                p.Id == TestDataConstants.ProductIdForUpdate);
        
        if (productExists is null) 
            Assert.Fail();

        // Act
        productExists.IsBlocked = true;
        productExists.OverallRating = 3;
        
        await _productRepository.UpdateAsync(productExists, CancellationToken.None);
        // Assert
        var productResult = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.ProductIdForUpdate);
        
        Assert.NotNull(productResult);
        Assert.Equal(productExists.IsBlocked, productResult.IsBlocked);
        Assert.Equal(productExists.OverallRating, productResult.OverallRating);
    }
    
    [Fact]
    public async Task UpdateProductAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        
        // Act
        var product = new Product()
        {
            Id = wrongId,
            IsBlocked = false,
            OverallRating = 5,
            SellerId = TestDataConstants.SellerIdForGetting4,
            CategoryId = TestDataConstants.CategoryIdForGetting5
        };
        // Assert
            await Assert.ThrowsAsync<NotFoundException>(async () =>
                await _productRepository.UpdateAsync(
                    product, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteProductAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Assert
        var productExists = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.ProductIdForDelete);
        Assert.NotNull(productExists);
        // Act
        await _productRepository.DeleteAsync(
            TestDataConstants.ProductIdForDelete, 
            CancellationToken.None);
        // Assert
        var productResult = await _context.Products
            .FirstOrDefaultAsync(s => 
                s.Id == TestDataConstants.ProductIdForDelete);
        
        Assert.Null(productResult);
    }
    
    [Fact]
    public async Task DeleteProductAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _productRepository.DeleteAsync(
                id: wrongId,
                CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllProductsAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var products = await _productRepository
            .FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(products);
        Assert.Equal(TestDataConstants.OverallProductsCount, products.Count());
    }
    
    [Fact]
    public async Task FindAllProductsAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var products = await _productRepository
            .FindAllAsync(
                cancellationToken: CancellationToken.None, 
                pageCount: 2);
        // Assert
        Assert.NotNull(products);
        Assert.Equal(2, products.Count());
    }
    
    [Fact]
    public async Task FindAllProductsAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var products = await _productRepository
            .FindAllAsync(
                cancellationToken: CancellationToken.None, 
                descending: false, 
                sortBy: ProductSortBy.Id);
        
        var result = products.ToList();
        // Assert
        Assert.NotNull(products);
        Assert.Equal(TestDataConstants.OverallProductsCount, products.Count());
        // TODO:
        /*Assert.Equal(TestDataConstants.SellerIdForUpdate , result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[1].Id);
        Assert.Equal(TestDataConstants.SellerIdForDelete, result[2].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting2, result[3].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting3, result[4].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting4, result[5].Id);#1#
    }
    
    [Fact]
    public async Task FindAllProductsAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var products = await _productRepository
            .FindAllAsync(
                cancellationToken: CancellationToken.None, 
                descending: false, 
                sortBy: ProductSortBy.Id);
        // Assert
        Assert.NotNull(products);
        Assert.Equal(TestDataConstants.OverallProductsCount, products.Count());
        // TODO:
        /*Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
        Assert.Equal(TestDataConstants.SellerIdForDelete, result[3].Id);
        Assert.Equal(TestDataConstants.SellerIdForGetting1, result[4].Id);
        Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);#1#
    }
    
    [Fact]
    public async Task FindAllProductsAsync_WithIsBlockedTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var products = await _productRepository
            .FindAllAsync(
                isBlocked: true,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(products);
        Assert.Equal(2, products.Count());
    }
    
    [Fact]
    public async Task FindAllProductsAsync_WithIsBlockedFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var products = await _productRepository
            .FindAllAsync(
                isBlocked: false,
                cancellationToken: CancellationToken.None);
        // Assert
        Assert.NotNull(products);
        Assert.Equal(TestDataConstants.OverallProductsCount - 2, products.Count());
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindProductByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var product = await _productRepository
            .FindByIdAsync(
                TestDataConstants.ProductIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(product);
        Assert.Equal(TestDataConstants.ProductIdForGetting1, product.Id);
    }
    
    [Fact]
    public async Task FindProductByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        // Act
        var product = await _productRepository
            .FindByIdAsync(
                id: wrongId, 
                CancellationToken.None);
        // Assert
        Assert.Null(product);
    }
    
    [Fact]
    public async Task GetProductByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var product = await _productRepository
            .GetByIdAsync(
                TestDataConstants.ProductIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(product);
        Assert.Equal(TestDataConstants.ProductIdForGetting1, product.Id);
    }
    
    [Fact]
    public async Task GetProductByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var wrongId = Guid.NewGuid();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _productRepository 
                .GetByIdAsync(
                    wrongId,
                    CancellationToken.None));
    }
    #endregion
    #region Full Page
    [Fact]
    public async Task FindFullProductPageById_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var productId = TestDataConstants.ProductIdForGetting6;
        // Act
        var result = await _productRepository.FindFullAsync(
            productId,
            CancellationToken.None);
        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Product);
        Assert.NotNull(result.Variants);
        Assert.NotNull(result.Seller);
        Assert.NotNull(result.Cloth);
        Assert.NotNull(result.ClothSizes);
        Assert.NotNull(result.Details);
        Assert.NotNull(result.Attributes);
        Assert.NotNull(result.Prices);
    }
    
    [Fact]
    public async Task FindFullProductPageById_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _productRepository = new ProductRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var productId = Guid.NewGuid();
        // Act
        var result = await _productRepository.FindFullAsync(
            productId,
            CancellationToken.None);
        // Assert
        Assert.Null(result);
    }
    #endregion
}*/