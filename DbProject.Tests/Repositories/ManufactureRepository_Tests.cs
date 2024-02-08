using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class ManufactureRepository_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void Create_Should_SaveManufactureToDatabase()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };

        //Act
        var result = manufactureRepository.Create(manufactureEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void Create_Should_Not_SaveManufactureToDatabase_ReturnNull()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture();

        //Act
        var result = manufactureRepository.Create(manufactureEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_Should_GetAllManufacturers_ReturnIEnumerableOfTypeManufactureEntity()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };
        manufactureRepository.Create(manufactureEntity);

        //Act
        var result = manufactureRepository.GetAll();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<Manufacture>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_GetOneManufactureFromDatabase_ReturnOneManufacture()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };
        manufactureRepository.Create(manufactureEntity);

        //Act
        var result = manufactureRepository.GetOne(x => x.ManufactureName == manufactureEntity.ManufactureName);

        //Assert
        Assert.Equal(manufactureEntity.Id, result.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneManufactureFromDatabase_ReturnNull()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };
        //manufactureRepository.Create(manufactureEntity);

        //Act
        var result = manufactureRepository.GetOne(x => x.ManufactureName == manufactureEntity.ManufactureName);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingManufacture_ReturnUpdatedManufactureEntity()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };
        manufactureRepository.Create(manufactureEntity);

        // Act
        manufactureEntity.ManufactureName = "TestManufacture";
        var result = manufactureRepository.Update(x => x.Id == manufactureEntity.Id, manufactureEntity);

        //Assert
        Assert.Equal("TestManufacture", result.ManufactureName);
        Assert.Equal(manufactureEntity.ManufactureName, result.ManufactureName);
        Assert.NotNull(result);
    }

    [Fact]
    public void Update_Should_Not_UpdateExistingManufacture_Returnnull()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };
        //manufactureRepository.Create(manufactureEntity);

        // Act
        manufactureEntity.ManufactureName = "TestManufacture";
        var result = manufactureRepository.Update(x => x.Id == manufactureEntity.Id, manufactureEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_Should_DeleteManufacureFromDatabase_ReturnTrue()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };
        manufactureRepository.Create(manufactureEntity);

        // Act
        var result = manufactureRepository.Delete(x => x.Id == manufactureEntity.Id);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_Should_Not_DeleteManufacureFromDatabase_ReturnFalse()
    {
        // Arrange
        var manufactureRepository = new ManufactureRepository(_productContext);
        var manufactureEntity = new Manufacture { ManufactureName = "Test" };
        //manufactureRepository.Create(manufactureEntity);

        // Act
        var result = manufactureRepository.Delete(x => x.Id == manufactureEntity.Id);

        //Assert
        Assert.False(result);
    }
}
