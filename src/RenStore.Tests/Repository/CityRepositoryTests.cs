/*using Microsoft.EntityFrameworkCore;
using RenStore.Delivery.Domain.Entities;
using RenStore.Delivery.Domain.Enums.Sorting;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using RenStore.SharedKernal.Domain.Exceptions;
using Tests.Common;

namespace Tests.Repository;

public class CityRepositoryTests
{
    private ApplicationDbContext _context;
    private CityRepository _cityRepository;

    #region Create Update Delete
    [Fact]
    public async Task CreateCityAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var city = new City()
        {
            Id = 1664633,
            Name = "Test",
            NormalizedName = "TEST",
            NameRu = "ТЕСЕ",
            NormalizedNameRu = "ТЕСЕ",
            CountryId = TestDataConstants.CountryIdForGetting1
        };
        // Act
        await _cityRepository.CreateAsync(city, CancellationToken.None);
        // Assert
        var result = await _context.Cities
            .FirstOrDefaultAsync(c => 
                c.Id == city.Id);
        Assert.NotNull(result);
        Assert.Equal(city.Id, result.Id);
        Assert.Equal(city.Name, result.Name);
        Assert.Equal(city.NormalizedName, result.NormalizedName);
        Assert.Equal(city.NameRu, result.NameRu);
        Assert.Equal(city.NormalizedNameRu, result.NormalizedNameRu);
        Assert.Equal(city.CountryId, result.CountryId);
    }
    
    [Fact]
    public async Task UpdateCityAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var city = await _context.Cities
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CityIdForUpdate);
        Assert.NotNull(city);
        string updatedName = Guid.NewGuid().ToString();
        string updatedNormalizedName = updatedName.ToUpper();
        string nameRu = Guid.NewGuid().ToString();
        string normalizedNameRu = nameRu.ToUpper();
        // Act
        city.Name = updatedName;
        city.NormalizedName = updatedNormalizedName;
        city.NameRu = nameRu;
        city.NormalizedNameRu = normalizedNameRu;
        await _cityRepository.UpdateAsync(city, CancellationToken.None);
        // Assert
        var result = await _context.Cities
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CityIdForUpdate);
        Assert.NotNull(result);
        Assert.Equal(city.Id, result.Id);
        Assert.Equal(updatedName, result.Name);
        Assert.Equal(updatedNormalizedName, result.NormalizedName);
        Assert.Equal(nameRu, result.NameRu);
        Assert.Equal(normalizedNameRu, result.NormalizedNameRu);
    }
    
    [Fact]
    public async Task UpdateCityAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var city = new City()
        {
            Id = 166264633,
            Name = "Test",
            NormalizedName = "TEST",
            NameRu = "ТЕСЕ",
            NormalizedNameRu = "ТЕСЕ",
            CountryId = TestDataConstants.CountryIdForGetting1
        };
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _cityRepository.UpdateAsync(
                city: city, 
                CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteCityAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        var city = await _context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CityIdForDelete);
        Assert.NotNull(city);
        // Act
        await _cityRepository.DeleteAsync(TestDataConstants.CityIdForDelete, CancellationToken.None);
        // Assert
        var cityExisting = await _context.Cities
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == TestDataConstants.CityIdForDelete);
        Assert.Null(cityExisting);
    }
    
    [Fact]
    public async Task DeleteCityAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int idForDelete = 4632352;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _cityRepository.DeleteAsync(
                id: idForDelete,
                CancellationToken.None));
    }
    #endregion
    #region Find All
    [Fact]
    public async Task FindAllCitiesAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var cities = await _cityRepository.FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(TestDataConstants.OverallCitiesCount, cities.Count());
    }

    [Fact]
    public async Task FindAllCitiesAsync_WithCountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var cities = await _cityRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            pageCount: 3);
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(3, cities.Count());
    }
    
    [Fact]
    public async Task FindAllCitiesAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _cityRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CitySortBy.Id);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(TestDataConstants.OverallCitiesCount, cities.Count());
        Assert.Equal(TestDataConstants.CityIdForUpdate, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForDelete, cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting1, cities[2].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting2, cities[3].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting3, cities[4].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[5].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[6].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[7].Id);
    }
    
    [Fact]
    public async Task FindAllCitiesAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
       _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _cityRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CitySortBy.Id);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(TestDataConstants.OverallCitiesCount, cities.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[2].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting3, cities[3].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting2, cities[4].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting1, cities[5].Id);
        Assert.Equal(TestDataConstants.CityIdForDelete, cities[6].Id);
        Assert.Equal(TestDataConstants.CityIdForUpdate, cities[7].Id);
    }
    
    [Fact]
    public async Task FindAllCitiesAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _cityRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CitySortBy.Name);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(TestDataConstants.OverallCitiesCount, cities.Count());
        
        Assert.Equal(TestDataConstants.CityIdForGetting3, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForDelete, cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting2, cities[2].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[3].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[4].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[5].Id);
        Assert.Equal(TestDataConstants.CityIdForUpdate, cities[6].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting1, cities[7].Id);
    }
    
    [Fact]
    public async Task FindAllCitiesAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _cityRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CitySortBy.Name);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(8, cities.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting1, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForUpdate , cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[2].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[3].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[4].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting2, cities[5].Id);
        Assert.Equal(TestDataConstants.CityIdForDelete, cities[6].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting3, cities[7].Id);
    }
    
    #endregion
    #region By Id
    [Fact]
    public async Task FindCityByIdAsync_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var city = await _cityRepository
            .FindByIdAsync(
                TestDataConstants.CityIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(city);
        Assert.Equal(TestDataConstants.CityIdForGetting1, city.Id);
    }

    [Fact]
    public async Task FindCityByIdAsync_FailOnWrongId_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 1435;
        // Act
        var city = await _cityRepository
            .FindByIdAsync(
                wrongId, 
                CancellationToken.None);
        // Assert
        Assert.Null(city);
    }
    
    [Fact]
    public async Task GetCityByIdAsync_Success_Test() 
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var city = await _cityRepository
            .GetByIdAsync(
                TestDataConstants.CityIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(city);
        Assert.Equal(TestDataConstants.CityIdForGetting1, city.Id);
    }
    
    [Fact]
    public async Task GetCityByIdAsync_FailOnWrongId_Test() 
    { 
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 143425;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _cityRepository
                .GetByIdAsync(
                    id: wrongId,
                    CancellationToken.None));
    }
    #endregion
    #region By Name

    [Fact]
    public async Task FindCitiesByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);_cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var cities = await _cityRepository
            .FindByNameAsync(
                name, 
                CancellationToken.None);
        var result = cities.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        
        Assert.Equal(TestDataConstants.CityIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, result[2].Id);
    }

    [Fact]
    public async Task FindCitiesByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        var countries = await _cityRepository
            .FindByNameAsync(
                name: wrongName, 
                CancellationToken.None);
        // Assert
        var result = countries.ToList();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCitiesByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var cities = await _cityRepository
            .GetByNameAsync(
                name, 
                CancellationToken.None);
        var result = cities.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, result[2].Id);
    }

    [Fact]
    public async Task GetCitiesByNameAsync_FailOnWrongName_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _cityRepository
                .GetByNameAsync(
                    name: wrongName, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task FindCitiesByNameAsync_CountLimit_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var cities = await _cityRepository
            .FindByNameAsync(
                pageCount: 2,
                name: name, 
                cancellationToken: CancellationToken.None);
        var result = cities.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, result[1].Id);
    }
    
    [Fact]
    public async Task FindCitiesByNameAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var result = await _cityRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CitySortBy.Name);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(3, cities.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[2].Id);
    }

    [Fact]
    public async Task FindCitiesByNameAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var result = await _cityRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CitySortBy.Name);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(3, cities.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[2].Id);
    }

    [Fact]
    public async Task FindCitiesByNameAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var result = await _cityRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CitySortBy.Id);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(3, cities.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[2].Id);
    }

    [Fact]
    public async Task FindCitiesByNameAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var result = await _cityRepository.FindByNameAsync(
            name: name,
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CitySortBy.Id);
        var cities = result.ToList();
        // Assert
        Assert.NotNull(cities);
        Assert.Equal(3, cities.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting6, cities[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, cities[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting4, cities[2].Id);
    }
    
    [Fact]
    public async Task FindCitiesByNameRuAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var cities = await _cityRepository
            .FindByNameAsync(
                name, 
                CancellationToken.None);
        var result = cities.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, result[2].Id);
    }
    
    [Fact]
    public async Task FindCitiesByOtherNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = TestDatabaseFixture.CreateReadyContext();
        _cityRepository = new CityRepository(_context, TestDatabaseFixture.ConnectionString);
        // Arrange
        string name = "mila";
        // Act
        var countries = await _cityRepository
            .FindByNameAsync(
                name: name, 
                CancellationToken.None);
        var result = countries.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CityIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CityIdForGetting6, result[2].Id);
    }
    #endregion
}*/