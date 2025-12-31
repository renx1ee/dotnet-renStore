using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using Tests.Common;

namespace Tests.Repository;

public class ProductAttributeRepositoryTest
{
    private ApplicationDbContext _context;
    private ProductAttributeRepository _productAttributeRepository;
    #region Create Update Delete
    [Fact]
    public async Task CreateProductAttributeAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arranges
       var productAttribute = new ProductAttributeEntity()
       {
           Id = Guid.NewGuid(),
           Name = "test",
           Value = "test",
           ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
       };
       // Act
       var result = await _productAttributeRepository.CreateAsync(productAttribute, CancellationToken.None);
       // Assert
       Assert.Equal(productAttribute.Id, result);
       var productAttributeExists = await _context.ProductAttributes
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == productAttribute.Id);
       
       Assert.NotNull(productAttributeExists);
       Assert.Equal(productAttribute.Id, productAttributeExists.Id);
       Assert.Equal(productAttribute.Name, productAttributeExists.Name);
       Assert.Equal(productAttribute.Value, productAttributeExists.Value);
       Assert.Equal(productAttribute.ProductVariantId, productAttributeExists.ProductVariantId);
    }

    [Fact]
    public async Task CreateProductAttributeAsync_FailOnEmpty_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var productAttribute = new ProductAttributeEntity();
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productAttributeRepository
               .CreateAsync(
                   productAttribute, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductAttributeAsync_FailOnWrongProductVariantId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var productAttribute = new ProductAttributeEntity()
       {
           Id = Guid.NewGuid(),
           Name = "test",
           Value = "test",
           ProductVariantId = Guid.NewGuid()
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() =>
           await _productAttributeRepository
               .CreateAsync(
                   productAttribute,
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductAttributeAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var productAttributeExists = await _context.ProductAttributes
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.ProductAttributeIdForUpdate);
       
       if (productAttributeExists is null) 
           Assert.Fail();
       
       productAttributeExists.Name = "Updated Name";
       productAttributeExists.Value = "Updated Name";
       
       await _productAttributeRepository.UpdateAsync(productAttributeExists, CancellationToken.None);
       // Assert
       var productAttributeResult = await _context.ProductAttributes
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductAttributeIdForUpdate);
       
       Assert.NotNull(productAttributeResult);
       Assert.Equal(productAttributeExists.Name, productAttributeResult.Name);
       Assert.Equal(productAttributeExists.Value, productAttributeResult.Value);
    }

    [Fact]
    public async Task UpdateProductAttributeAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productAttribute = new ProductAttributeEntity()
       {
           Id = Guid.NewGuid(),
           Name = "test",
           Value = "test",
           ProductVariantId = Guid.NewGuid()
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _productAttributeRepository.UpdateAsync(
                   productAttribute, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProductAttributeAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Assert
       var productAttributeExists = await _context.ProductAttributes
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductAttributeIdForDelete);
       Assert.NotNull(productAttributeExists);
       // Act
       await _productAttributeRepository.DeleteAsync(
           TestDataConstants.ProductAttributeIdForDelete, 
           CancellationToken.None);
       // Assert
       var productAttributeResult = await _context.ProductAttributes
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductAttributeIdForDelete);
       Assert.Null(productAttributeResult);
    }

    [Fact]
    public async Task DeleteProductAttributeAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productAttributeRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllProductAttributesAsync_WithDefaultParameters_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productAttributes = await _productAttributeRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(productAttributes);
       Assert.Equal(TestDataConstants.OverallProductAttributeCount, productAttributes.Count());
    }

    [Fact]
    public async Task FindAllProductAttributesAsync_CountLimit_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productAttributes = await _productAttributeRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(productAttributes);
       Assert.Equal(2, productAttributes.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllProductAttributesAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productAttributes = await _productAttributeRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductAttributeSortBy.Id);
       // Assert
       Assert.NotNull(productAttributes);
       Assert.Equal(TestDataConstants.OverallProductAttributeCount, productAttributes.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllProductAttributesAsync_SortById_DescendingTrue_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productAttributes = await _productAttributeRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductAttributeSortBy.Id);
       // Assert
       Assert.NotNull(productAttributes);
       Assert.Equal(TestDataConstants.OverallProductAttributeCount, productAttributes.Count());
       // TODO:
       /*Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
       Assert.Equal(TestDataConstants.SellerIdForDelete, result[3].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting1, result[4].Id);
       Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);*/
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindProductAttributeByIdAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productAttribute = await _productAttributeRepository
           .FindByIdAsync(
               TestDataConstants.ProductAttributeIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productAttribute);
       Assert.Equal(TestDataConstants.ProductAttributeIdForGetting1, productAttribute.Id);
    }

    [Fact]
    public async Task FindProductAttributeByIdAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var productAttribute = await _productAttributeRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(productAttribute);
    }

    [Fact]
    public async Task GetProductAttributeByIdAsync_Success_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productAttribute = await _productAttributeRepository
           .GetByIdAsync(
               TestDataConstants.ProductAttributeIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productAttribute);
       Assert.Equal(TestDataConstants.ProductAttributeIdForGetting1, productAttribute.Id);
    }

    [Fact]
    public async Task GetProductAttributeByIdAsync_FailOnWrongId_Test()
    {
       _context = DatabaseFixture.CreateReadyContext();
       _productAttributeRepository = new ProductAttributeRepository(_context, DatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productAttributeRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}