/*using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class ProductDetailRepositoryTests
{
    private ApplicationDbContext _context;
    private ProductDetailRepository _productDetailRepository;
    #region Create Edit Delete
    [Fact]
    public async Task CreateProductDetailAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arranges
       var productDetail = new ProductDetailEntity()
       {
           Id = Guid.NewGuid(),
           Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Composition = "Composition",
           CaringOfThings = "CaringOfThings",
           TypeOfPacking = TypeOfPackaging.Box,
           CountryOfManufactureId = TestDataConstants.CountryIdForGetting1,
           ProductVariantId = TestDataConstants.ProductVariantIdForGetting7
       };
       // Act
       var result = await _productDetailRepository.CreateAsync(productDetail, CancellationToken.None);
       // Assert
       Assert.Equal(productDetail.Id, result);
       var productDetailExists = await _context.ProductDetails
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == productDetail.Id);
       
       Assert.NotNull(productDetailExists);
       Assert.Equal(productDetail.Id, productDetailExists.Id);
       Assert.Equal(productDetail.Description, productDetailExists.Description);
       Assert.Equal(productDetail.ModelFeatures, productDetailExists.ModelFeatures);
       Assert.Equal(productDetail.DecorativeElements, productDetailExists.DecorativeElements);
       Assert.Equal(productDetail.Equipment, productDetailExists.Equipment);
       Assert.Equal(productDetail.Composition, productDetailExists.Composition);
       Assert.Equal(productDetail.CaringOfThings, productDetailExists.CaringOfThings);
       Assert.Equal(productDetail.TypeOfPacking, productDetailExists.TypeOfPacking);
       Assert.Equal(productDetail.CountryOfManufactureId, productDetailExists.CountryOfManufactureId);
       Assert.Equal(productDetail.ProductVariantId, productDetailExists.ProductVariantId);
    }

    [Fact]
    public async Task CreateProductDetailAsync_FailOnEmpty_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productDetail = new ProductDetailEntity();
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productDetailRepository
               .CreateAsync(
                   productDetail, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductDetailAsync_FailOnWrongProductVariantId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productDetail = new ProductDetailEntity()
       {
           Id = Guid.NewGuid(),
           Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Composition = "Composition",
           CaringOfThings = "CaringOfThings",
           TypeOfPacking = TypeOfPackaging.Box,
           CountryOfManufactureId = TestDataConstants.CountryIdForGetting1,
           ProductVariantId = Guid.NewGuid()
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productDetailRepository
               .CreateAsync(
                   productDetail, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task CreateProductDetailAsync_FailOnWrongCountryId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productDetail = new ProductDetailEntity()
       {
           Id = Guid.NewGuid(),
           Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Composition = "Composition",
           CaringOfThings = "CaringOfThings",
           TypeOfPacking = TypeOfPackaging.Box,
           CountryOfManufactureId = 87542782,
           ProductVariantId = TestDataConstants.ProductVariantIdForGetting1
       };
       // Act & Assert
       await Assert.ThrowsAsync<DbUpdateException>(async() => 
           await _productDetailRepository
               .CreateAsync(
                   productDetail, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task UpdateProductDetailAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var productDetailExists = await _context.ProductDetails
           .AsNoTracking()
           .FirstOrDefaultAsync(p => 
               p.Id == TestDataConstants.ProductDetailIdForUpdate);
       
       if (productDetailExists is null) 
           Assert.Fail();
       
       productDetailExists.Description = "Updated Description";
       productDetailExists.ModelFeatures = "Updated Model features";
       productDetailExists.DecorativeElements = "Updated kefklqwklfkl";
       productDetailExists.Equipment = "Updated Equipment";
       productDetailExists.Composition = "Updated Composition";
       productDetailExists.CaringOfThings = "Updated CaringOfThings";
       productDetailExists.TypeOfPacking =  TypeOfPackaging.Package;
       
       await _productDetailRepository.UpdateAsync(productDetailExists, CancellationToken.None);
       // Assert
       var productDetailResult = await _context.ProductDetails
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductDetailIdForUpdate);
       
       Assert.NotNull(productDetailResult);
       Assert.Equal(productDetailExists.Description, productDetailResult.Description);
       Assert.Equal(productDetailExists.ModelFeatures, productDetailResult.ModelFeatures);
       Assert.Equal(productDetailExists.DecorativeElements, productDetailResult.DecorativeElements);
       Assert.Equal(productDetailExists.Equipment, productDetailResult.Equipment);
       Assert.Equal(productDetailExists.Composition, productDetailResult.Composition);
       Assert.Equal(productDetailExists.CaringOfThings, productDetailResult.CaringOfThings);
       Assert.Equal(productDetailExists.TypeOfPacking, productDetailResult.TypeOfPacking);
    }

    [Fact]
    public async Task UpdateProductDetailAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productDetail = new ProductDetailEntity()
       {
           Id = Guid.NewGuid(),
           Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           ModelFeatures = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           DecorativeElements = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Equipment = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Commodi placeat minima recusandae libero earum praesentium aspernatur, ipsam sunt. Nostrum ipsum sapiente corporis porro debitis perferendis autem enim nulla impedit cumque.",
           Composition = "Composition",
           CaringOfThings = "CaringOfThings",
           TypeOfPacking = TypeOfPackaging.Box,
           CountryOfManufactureId = TestDataConstants.CountryIdForGetting1,
           ProductVariantId = Guid.NewGuid()
       };
       // Assert
           await Assert.ThrowsAsync<NotFoundException>(async () =>
               await _productDetailRepository.UpdateAsync(
                   productDetail, 
                   CancellationToken.None));
    }

    [Fact]
    public async Task DeleteProductDetailAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Assert
       var productDetailExists = await _context.ProductDetails
           .AsNoTracking()
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductDetailIdForDelete);
       Assert.NotNull(productDetailExists);
       // Act
       await _productDetailRepository.DeleteAsync(
           TestDataConstants.ProductDetailIdForDelete, 
           CancellationToken.None);
       // Assert
       var productDetailResult = await _context.ProductDetails
           .FirstOrDefaultAsync(s => 
               s.Id == TestDataConstants.ProductDetailIdForDelete);
       Assert.Null(productDetailResult);
    }

    [Fact]
    public async Task DeleteProductDetailAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productDetailRepository.DeleteAsync(
               id: wrongId,
               CancellationToken.None));
    }
    #endregion
    #region All
    [Fact]
    public async Task FindAllProductDetailsAsync_WithDefaultParameters_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productDetails = await _productDetailRepository
           .FindAllAsync(CancellationToken.None);
       // Assert
       Assert.NotNull(productDetails);
       Assert.Equal(TestDataConstants.OverallProductDetailsCount, productDetails.Count());
    }

    [Fact]
    public async Task FindAllProductDetailsAsync_CountLimit_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productDetails = await _productDetailRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               pageCount: 2);
       // Assert
       Assert.NotNull(productDetails);
       Assert.Equal(2, productDetails.Count());
    }
    // TODO:
    [Fact]
    public async Task FindAllProductDetailsAsync_SortById_DescendingFalse_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productDetails = await _productDetailRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductDetailSortBy.Id);
       // Assert
       Assert.NotNull(productDetails);
       Assert.Equal(TestDataConstants.OverallProductDetailsCount, productDetails.Count());
       // TODO:
    }
    // TODO:
    [Fact]
    public async Task FindAllProductDetailsAsync_SortById_DescendingTrue_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productDetails = await _productDetailRepository
           .FindAllAsync(
               cancellationToken: CancellationToken.None, 
               descending: false, 
               sortBy: ProductDetailSortBy.Id);
       // Assert
       Assert.NotNull(productDetails);
       Assert.Equal(TestDataConstants.OverallProductDetailsCount, productDetails.Count());
       // TODO:
       /*Assert.Equal(TestDataConstants.SellerIdForGetting4 , result[0].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting3, result[1].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting2, result[2].Id);
       Assert.Equal(TestDataConstants.SellerIdForDelete, result[3].Id);
       Assert.Equal(TestDataConstants.SellerIdForGetting1, result[4].Id);
       Assert.Equal(TestDataConstants.SellerIdForUpdate, result[5].Id);#1#
    }
    #endregion
    #region By Id
    [Fact]
    public async Task FindProductDetailByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productDetail = await _productDetailRepository
           .FindByIdAsync(
               TestDataConstants.ProductDetailIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productDetail);
       Assert.Equal(TestDataConstants.ProductDetailIdForGetting1, productDetail.Id);
    }

    [Fact]
    public async Task FindProductDetailByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       var productDetail = await _productDetailRepository
           .FindByIdAsync(
               id: wrongId, 
               CancellationToken.None);
       // Assert
       Assert.Null(productDetail);
    }

    [Fact]
    public async Task GetProductDetailByIdAsync_Success_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       // Act
       var productDetail = await _productDetailRepository
           .GetByIdAsync(
               TestDataConstants.ProductDetailIdForGetting1, 
               CancellationToken.None);
       // Assert
       Assert.NotNull(productDetail);
       Assert.Equal(TestDataConstants.ProductDetailIdForGetting1, productDetail.Id);
    }

    [Fact]
    public async Task GetProductDetailByIdAsync_FailOnWrongId_Test()
    {
       _context = TestDatabaseFixture.CreateReadyContext();
       _productDetailRepository = new ProductDetailRepository(_context, TestDatabaseFixture.ConnectionString);
       // Arrange
       var wrongId = Guid.NewGuid();
       // Act
       // Assert
       await Assert.ThrowsAsync<NotFoundException>(async () =>
           await _productDetailRepository 
               .GetByIdAsync(
                   wrongId,
                   CancellationToken.None));
    }
    #endregion
}*/