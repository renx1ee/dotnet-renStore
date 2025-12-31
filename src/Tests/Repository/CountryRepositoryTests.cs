using Microsoft.EntityFrameworkCore;
using RenStore.Domain.Entities;
using RenStore.Domain.Enums.Sorting;
using RenStore.Domain.Exceptions;
using RenStore.Persistence;
using RenStore.Persistence.Repository.Postgresql;
using Tests.Common;

namespace Tests.Repository;

public class CountryRepositoryTests : IDisposable
{
    private ApplicationDbContext _context;
    private CountryRepository _countryRepository;

    #region Create Update Delete
    [Fact]
    public async Task CreateCountryAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var country = new CountryEntity()
        {
            Id = 163,
            Name = "Test",
            NormalizedName = "TEST",
            NameRu = "ТЕСЕ",
            Code =  "tes"
        };
        // Act
        var returnsId = await _countryRepository.CreateAsync(country, CancellationToken.None);
        // Assert
        var result = await _context.Countries
            .FirstOrDefaultAsync(c => 
                c.Id == country.Id);
        
        Assert.NotNull(result);
        Assert.Equal(country.Id, result.Id);
        Assert.Equal(country.Name, result.Name);
        Assert.Equal(country.NormalizedName, result.NormalizedName);
        Assert.Equal(country.NameRu, result.NameRu);
        Assert.Equal(country.Code, result.Code);
    }
    
    [Fact]
    public async Task CreateCountryAsync_FailOnDuplicateName_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        int id = 6326;
        var country = new CountryEntity()
        {
            Id = id,
            Name = "Getting1",
            NormalizedName = "GETTING_1",
            NameRu = "ТЕСТ3",
            Code = "get1"
        };
        // Act
        
        // Assert
        await Assert.ThrowsAsync<DbUpdateException>(async () =>
            await _countryRepository.CreateAsync(
                country: country, 
                CancellationToken.None));
    }
    
    [Fact]
    public async Task UpdateCountryAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        int idForUpdate = TestDataConstants.CountryIdForUpdate;
        var country = await _context.Countries
            .FirstOrDefaultAsync(c => 
                c.Id == idForUpdate);
        Assert.NotNull(country);
        string updatedName = Guid.NewGuid().ToString();
        string updatedNormalizedName = updatedName.ToUpper();
        string nameRu = Guid.NewGuid().ToString();
        string code = "eges";
        // Act
        country.Name = updatedName;
        country.NormalizedName = updatedNormalizedName;
        country.NameRu = nameRu;
        country.Code = code;
        await _countryRepository.UpdateAsync(country, CancellationToken.None);
        // Assert
        var result = await _context.Countries
            .FirstOrDefaultAsync(c => 
                c.Id == idForUpdate);
        
        Assert.NotNull(result);
        Assert.Equal(country.Id, result.Id);
        Assert.Equal(updatedName, result.Name);
        Assert.Equal(updatedNormalizedName, result.NormalizedName);
        Assert.Equal(nameRu, result.NameRu);
        Assert.Equal(code, result.Code);
    }
    
    [Fact]
    public async Task UpdateCountryAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        var country = new CountryEntity()
        {
            Id = 163,
            Name = "Germany",
            NormalizedName = "GERMANY",
            NameRu = "Германия",
            Code =  "de"
        };
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _countryRepository.UpdateAsync(
                country: country, 
                CancellationToken.None));
    }
    
    [Fact]
    public async Task DeleteCountryAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        int idForDelete = TestDataConstants.CountryIdForDelete;
        var country = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == idForDelete);
        Assert.NotNull(country);
        // Act
        await _countryRepository.DeleteAsync(idForDelete, CancellationToken.None);
        // Assert
        var countryExisting = await _context.Countries
            .AsNoTracking()
            .FirstOrDefaultAsync(c => 
                c.Id == idForDelete);
        Assert.Null(countryExisting);
    }
    
    [Fact]
    public async Task DeleteCountryAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        int idForDelete = 4632352;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _countryRepository.DeleteAsync(
                id: idForDelete,
                CancellationToken.None));
    }
    #endregion
    #region Find All
    [Fact]
    public async Task FindAllCountriesAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var countries = await _countryRepository.FindAllAsync(CancellationToken.None);
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(8, countries.Count());
    }

    [Fact]
    public async Task FindAllCountriesAsync_WithCountLimit_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var countries = await _countryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            pageCount: 3);
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(3, countries.Count());
    }
    
    [Fact]
    public async Task FindAllCountriesAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CountrySortBy.Id);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(8, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForUpdate, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForDelete, countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting1, countries[2].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting2, countries[3].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting3, countries[4].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[5].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[6].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[7].Id);
    }
    
    [Fact]
    public async Task FindAllCountriesAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CountrySortBy.Id);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(8, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[2].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting3, countries[3].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting2, countries[4].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting1, countries[5].Id);
        Assert.Equal(TestDataConstants.CountryIdForDelete, countries[6].Id);
        Assert.Equal(TestDataConstants.CountryIdForUpdate, countries[7].Id);
    }
    
    [Fact]
    public async Task FindAllCountriesAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CountrySortBy.Name);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(8, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForUpdate, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForDelete , countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting1, countries[2].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting2, countries[3].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting3, countries[4].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[5].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[6].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[7].Id);
    }
    
    [Fact]
    public async Task FindAllCountriesAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindAllAsync(
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CountrySortBy.Name);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(8, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[2].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting3, countries[3].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting2, countries[4].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting1, countries[5].Id);
        Assert.Equal(TestDataConstants.CountryIdForDelete, countries[6].Id);
        Assert.Equal(TestDataConstants.CountryIdForUpdate, countries[7].Id);
    }
    
    #endregion
    #region By Id
    [Fact]
    public async Task FindCountryByIdAsync_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var country = await _countryRepository
            .FindByIdAsync(
                TestDataConstants.CountryIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(country);
        Assert.Equal(TestDataConstants.CountryIdForGetting1, country.Id);
    }

    [Fact]
    public async Task FindCountryByIdAsync_FailOnWrongId_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 1435;
        // Act
        var country = await _countryRepository
            .FindByIdAsync(
                wrongId, 
                CancellationToken.None);
        // Assert
        Assert.Null(country);
    }
    
    [Fact]
    public async Task GetCountryByIdAsync_Success_Test() 
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var country = await _countryRepository
            .GetByIdAsync(
                TestDataConstants.CountryIdForGetting1, 
                CancellationToken.None);
        // Assert
        Assert.NotNull(country);
        Assert.Equal(TestDataConstants.CountryIdForGetting1, country.Id);
    }
    
    [Fact]
    public async Task GetCountryByIdAsync_FailOnWrongId_Test() 
    { 
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        int wrongId = 143425;
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await _countryRepository
                .GetByIdAsync(
                    id: wrongId,
                    CancellationToken.None));
    }
    
    #endregion
    #region By Name

    [Fact]
    public async Task FindCountriesByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var countries = await _countryRepository
            .FindByNameAsync(
                TestDataConstants.CountryNameForGetting4, 
                CancellationToken.None);
        var result = countries.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, result[2].Id);
    }

    [Fact]
    public async Task FindCountriesByNameAsync_FailOnWrongName_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        var countries = await _countryRepository
            .FindByNameAsync(
                name: wrongName, 
                CancellationToken.None);
        // Assert
        var result = countries.ToList();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCountriesByNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var countries = await _countryRepository
            .GetByNameAsync(
                TestDataConstants.CountryNameForGetting4, 
                CancellationToken.None);
        var result = countries.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, result[2].Id);
    }

    [Fact]
    public async Task GetCountriesByNameAsync_FailOnWrongName_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        string wrongName = Guid.NewGuid().ToString();
        // Act
        // Assert
        await Assert.ThrowsAsync<NotFoundException>(async () => 
            await _countryRepository
                .GetByNameAsync(
                    name: wrongName, 
                    CancellationToken.None));
    }
    
    [Fact]
    public async Task FindCountriesByNameAsync_CountLimit_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var countries = await _countryRepository
            .FindByNameAsync(
                pageCount: 2,
                name: TestDataConstants.CountryNameForGetting4, 
                cancellationToken: CancellationToken.None);
        var result = countries.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, result[1].Id);
    }
    
    [Fact]
    public async Task FindCountriesByNameAsync_SortByName_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindByNameAsync(
            name: TestDataConstants.CountryNameForGetting4,
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CountrySortBy.Name);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(3, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[2].Id);
    }

    [Fact]
    public async Task FindCountriesByNameAsync_SortByName_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindByNameAsync(
            name: TestDataConstants.CountryNameForGetting4,
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CountrySortBy.Name);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(3, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[2].Id);
    }

    [Fact]
    public async Task FindCountriesByNameAsync_SortById_DescendingFalse_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindByNameAsync(
            name: TestDataConstants.CountryNameForGetting4,
            cancellationToken: CancellationToken.None, 
            descending: false,
            sortBy: CountrySortBy.Id);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(3, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[2].Id);
    }

    [Fact]
    public async Task FindCountriesByNameAsync_SortById_DescendingTrue_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var result = await _countryRepository.FindByNameAsync(
            name: TestDataConstants.CountryNameForGetting4,
            cancellationToken: CancellationToken.None, 
            descending: true,
            sortBy: CountrySortBy.Id);
        var countries = result.ToList();
        // Assert
        Assert.NotNull(countries);
        Assert.Equal(3, countries.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting6, countries[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, countries[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting4, countries[2].Id);
    }
    
    [Fact]
    public async Task FindCountriesByNameRuAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var countries = await _countryRepository
            .FindByNameAsync(
                TestDataConstants.CountryNameRuForGetting4, 
                CancellationToken.None);
        var result = countries.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, result[2].Id);
    }
    
    [Fact]
    public async Task FindCountriesByOtherNameAsync_WithDefaultParameters_Success_Test()
    {
        _context = DatabaseFixture.CreateReadyContext();
        _countryRepository = new CountryRepository(_context, DatabaseFixture.ConnectionString);
        // Arrange
        // Act
        var countries = await _countryRepository
            .FindByNameAsync(
                TestDataConstants.CountryOtherNameForGetting6, 
                CancellationToken.None);
        var result = countries.ToList();
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count());
        Assert.Equal(TestDataConstants.CountryIdForGetting4, result[0].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting5, result[1].Id);
        Assert.Equal(TestDataConstants.CountryIdForGetting6, result[2].Id);
    }
    #endregion
    
    public void Dispose()
    {
        /*_context.Dispose()
        _context.Database.EnsureDeleted();*/
    }
}