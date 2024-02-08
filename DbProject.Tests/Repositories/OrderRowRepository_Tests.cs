using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class OrderRowRepository_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void Create_Should_SaveOrderRowToDatabase()
    {
        //Arrange
        var orderRowRepository = new OrderRowRepository(_productContext);
        var orderRowEntity = new OrderRow { Quantity = 12 };

        //Act
        var result = orderRowRepository.Create(orderRowEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void GetAll_Should_GetAllOrderRows_ReturnIEnumerableOfTypeOrderRowEntity()
    {
        //Arrange
        var orderRowRepository = new OrderRowRepository(_productContext);
        var orderRowEntity = new OrderRow { Quantity = 12 };
        orderRowRepository.Create(orderRowEntity);

        //Act
        var result = orderRowRepository.GetAll();

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_GetOneOrderRowFromDatabase_ReturnOneOrderRow()
    {
        //Arrange
        var orderRowRepository = new OrderRowRepository(_productContext);
        var productRepository = new ProductRepository(_productContext);
        var orderRepository = new OrderRepository(_productContext);

        var orderEntity = new Order();
        var productEntity = new Product { ProductName = "Test", ProductPrice = 12  };

        productRepository.Create(productEntity);
        orderRepository.Create(orderEntity);

        var orderRowEntity = new OrderRow { Quantity = 12, ProductId = productEntity.Id, OrderId = orderEntity.Id };
        orderRowRepository.Create(orderRowEntity);

        //Act
        var result = orderRowRepository.GetOne(x => x.Quantity == orderRowEntity.Quantity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(orderRowEntity.Id, result.Id);
        Assert.Equal(orderRowEntity.ProductId, result.Product.Id);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneOrderRowFromDatabase_ReturnNull()
    {
        //Arrange
        var orderRowRepository = new OrderRowRepository(_productContext);

        var orderRowEntity = new OrderRow { Quantity = 12 };
        orderRowRepository.Create(orderRowEntity);

        //Act
        var result = orderRowRepository.GetOne(x => x.Quantity == orderRowEntity.Quantity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingOrderRow_ReturnUpdatedOrderRowEntity()
    {
        //Arrange
        var orderRowRepository = new OrderRowRepository(_productContext);

        var orderRowEntity = new OrderRow { Quantity = 12 };
        orderRowRepository.Create(orderRowEntity);

        //Act
        orderRowEntity.Quantity = 2;
        var result = orderRowRepository.Update(x => x.Id == orderRowEntity.Id, orderRowEntity);

        //Assert
        Assert.Equal(orderRowEntity.Quantity, result.Quantity);
        Assert.NotNull(result);
    }

    [Fact]
    public void Update_Should_Not_UpdateExistingOrderRow_ReturnNull()
    {
        //Arrange
        var orderRowRepository = new OrderRowRepository(_productContext);

        var orderRowEntity = new OrderRow { Quantity = 12 };
        //orderRowRepository.Create(orderRowEntity);

        //Act
        orderRowEntity.Quantity = 2;
        var result = orderRowRepository.Update(x => x.Id == orderRowEntity.Id, orderRowEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_Should_DelteOrderRowFromDatabase_ReturnTrue()
    {
        //Arrange
        var orderRowRepository = new OrderRowRepository(_productContext);

        var orderRowEntity = new OrderRow { Quantity = 12 };
        orderRowRepository.Create(orderRowEntity);

        //Act
        var result = orderRowRepository.Delete(x => x.Id == orderRowEntity.Id);

        //Assert
        Assert.True(result);
    }
}
