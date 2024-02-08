using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class ProductRepository_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);


    [Fact]
    public void Create_Should_SaveProductToDatabase()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var productEntity = new Product { ProductName = "Test", ProductPrice = 10 };

        //Act
        var result = productRepository.Create(productEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, productEntity.Id);
    }

    [Fact]
    public void GetAll_Should_GetAllProducts_ReturnIEnumerableOfTypeProduct()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var productEntity = new Product { ProductName = "Test", ProductPrice = 10 };
        productRepository.Create(productEntity);

        //Act
        var result = productRepository.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Product>>(result);
    }

    [Fact]
    public void GetOne_Should_GetOneProductFromDatabase_ReturnOneProduct()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var categoryRepository = new CategoryRepository(_productContext);
        var manufactureRepository = new ManufactureRepository(_productContext);
        var descriptionRepository = new DescriptionRepository(_productContext);

        var categoryEntity = new Category { CategoryName = "Test" };
        categoryRepository.Create(categoryEntity);

        var manufacruteEntity = new Manufacture { ManufactureName = "Test" };
        manufactureRepository.Create(manufacruteEntity);

        var descriptionEntity = new Description { Ingress = "Test" };
        descriptionRepository.Create(descriptionEntity);

        var productEntity = new Product { ProductName = "Test", ProductPrice = 10, DescriptionId = descriptionEntity.Id, CategoryId = categoryEntity.Id, ManufactureId = manufacruteEntity.Id  };
        productRepository.Create(productEntity);

        //Act
        var result = productRepository.GetOne(x => x.Id == productEntity.Id);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneProductFromDatabase_ReturnNull()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var categoryRepository = new CategoryRepository(_productContext);
        var manufactureRepository = new ManufactureRepository(_productContext);
        var descriptionRepository = new DescriptionRepository(_productContext);

        var categoryEntity = new Category { CategoryName = "Test" };
        categoryRepository.Create(categoryEntity);

        var manufacruteEntity = new Manufacture { ManufactureName = "Test" };
        manufactureRepository.Create(manufacruteEntity);

        //var descriptionEntity = new Description { Ingress = "Test" };
        //descriptionRepository.Create(descriptionEntity);

        var productEntity = new Product { ProductName = "Test", ProductPrice = 10, CategoryId = categoryEntity.Id, ManufactureId = manufacruteEntity.Id };
        productRepository.Create(productEntity);

        //Act
        var result = productRepository.GetOne(x => x.Id == productEntity.Id);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingProduct_ReturnUpdatedProduct()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);

        var productEntity = new Product { ProductName = "Test", ProductPrice = 10 };
        productRepository.Create(productEntity);

        //Act
        productEntity.ProductName = "Tester";
        var result = productRepository.Update(x => x.Id == productEntity.Id, productEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void Delete_Should_DeleteProductFrom_ReturnTrue()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var reviewRepository = new ReviewRepository(_productContext);
        var descriptionRepository = new DescriptionRepository(_productContext);

        var descriptionEntity = new Description { Ingress = "Test" };
        descriptionRepository.Create(descriptionEntity);

        var productEntity = new Product { ProductName = "Test", ProductPrice = 10, DescriptionId = descriptionEntity.Id };
        productRepository.Create(productEntity);

        //Act
        var result = productRepository.Delete(x => x.Id == productEntity.Id);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_Should_Not_DeleteProductFrom_ReturnTrue()
    {
        //Arrange
        var productRepository = new ProductRepository(_productContext);
        var reviewRepository = new ReviewRepository(_productContext);
        var descriptionRepository = new DescriptionRepository(_productContext);

        var descriptionEntity = new Description { Ingress = "Test" };
        //descriptionRepository.Create(descriptionEntity);

        var productEntity = new Product { ProductName = "Test", ProductPrice = 10, DescriptionId = descriptionEntity.Id };
        productRepository.Create(productEntity);

        //Act
        var result = productRepository.Delete(x => x.Id == productEntity.Id);

        //Assert
        Assert.False(result);
    }
}
