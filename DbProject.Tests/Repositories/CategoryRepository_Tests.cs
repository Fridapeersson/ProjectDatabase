using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class CategoryRepository_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void Create_Should_SaveCategoryToDatabase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };

        //Act
        var result = categoryRepository.Create(categoryEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void Create_Should_Not_SaveCategoryToDatabase_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category();

        //Act
        var result = categoryRepository.Create(categoryEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_Should_GetAllCategoriesFromDatabase_ReturnIEnumerableOfTypeCategoryEntity()
    {
        //Arrange
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };
        categoryRepository.Create(categoryEntity);

        //Act
        var result = categoryRepository.GetAll();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<Category>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_GetOneCategory_ReturnOneCategory()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };
        categoryRepository.Create(categoryEntity);

        //Act
        var result = categoryRepository.GetOne(x => x.Id == categoryEntity.Id);

        //Assert
        Assert.Equal(1, categoryEntity.Id);
        Assert.Equal(result.CategoryName, categoryEntity.CategoryName);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneCategory_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };
        //categoryRepository.Create(categoryEntity);

        //Act
        var result = categoryRepository.GetOne(x => x.Id == categoryEntity.Id);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingCategory_ReturnUpdatedCategoryEntity()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };
        categoryRepository.Create(categoryEntity);

        //Act
        categoryEntity.CategoryName = "Testing";
        var result = categoryRepository.Update(x => x.Id == categoryEntity.Id, categoryEntity);

        //Assert
        Assert.Equal("Testing", categoryEntity.CategoryName);
        Assert.Equal(categoryEntity.CategoryName, result.CategoryName);
        Assert.NotNull(result);
    }

    [Fact]
    public void Update_Should_Not_UpdateExistingCategory_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };
        //categoryRepository.Create(categoryEntity);

        //Act
        categoryEntity.CategoryName = "Testing";
        var result = categoryRepository.Update(x => x.Id == categoryEntity.Id, categoryEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_Should_DeleteExistingCategory_ReturnTrue() 
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };
        categoryRepository.Create(categoryEntity);

        //Act
        var result = categoryRepository.Delete(x => x.Id == categoryEntity.Id);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_Should_Not_DeleteExistingCategory_ReturnFalse()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryEntity = new Category { CategoryName = "Test" };
        //categoryRepository.Create(categoryEntity);

        //Act
        var result = categoryRepository.Delete(x => x.Id == categoryEntity.Id);

        //Assert
        Assert.False(result);
    }
}
