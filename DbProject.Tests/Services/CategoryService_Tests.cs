using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Services;

public class CategoryService_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void CreateCategory_Should_SaveCategoryToDatabase()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryService = new CategoryService(categoryRepository);
        var categoryEntity = new Category { CategoryName = "Test" };

        //Act
        var result = categoryService.CreateCategory(categoryEntity);

        //Assert
        Assert.Equal("Test", result.CategoryName);
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void CreateCategory_Should_Not_SaveCategoryToDatabase_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryService = new CategoryService(categoryRepository);
        var categoryEntity = new Category();

        //Act
        var result = categoryService.CreateCategory(categoryEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAllCategories_Should_GetAllCategories_ReturnIEnumerableOfTypeCategory()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryService = new CategoryService(categoryRepository);
        var categoryEntity = new Category { CategoryName = "Test" };
        categoryService.CreateCategory(categoryEntity);

        //Act
        var result = categoryService.GetAllCategories();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<Category>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOneCategory_Should_GetOneCategory_ReturnOneCategory()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryService = new CategoryService(categoryRepository);
        var categoryEntity = new Category { CategoryName = "Test" };
        categoryService.CreateCategory(categoryEntity);

        //Act
        var result = categoryService.GetOneCategory(x => x.CategoryName == categoryEntity.CategoryName);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Test", result.CategoryName);
    }

    [Fact]
    public void GetOneCategory_Should_Not_GetOneCategory_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryService = new CategoryService(categoryRepository);
        var categoryEntity = new Category { CategoryName = "Test" };
        //categoryService.CreateCategory(categoryEntity);

        //Act
        var result = categoryService.GetOneCategory(x => x.CategoryName == categoryEntity.CategoryName);

        //Assert
        Assert.Null(result);
    }


    [Fact]
    public void DeleteCategory_Should_DeleteCategory_ReturnTrue()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryService = new CategoryService(categoryRepository);
        var categoryEntity = new Category { CategoryName = "Test" };
        categoryService.CreateCategory(categoryEntity);

        //Act
        var result = categoryService.DeleteCategory(x => x.CategoryName == categoryEntity.CategoryName);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void DeleteCategory_Should_Not_DeleteCategory_ReturnFalse()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_productContext);
        var categoryService = new CategoryService(categoryRepository);
        var categoryEntity = new Category { CategoryName = "Test" };
        //categoryService.CreateCategory(categoryEntity);

        //Act
        var result = categoryService.DeleteCategory(x => x.CategoryName == categoryEntity.CategoryName);

        //Assert
        Assert.False(result);
    }
}
