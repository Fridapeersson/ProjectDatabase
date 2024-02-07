using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class OrderRepository_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);


    [Fact]
    public void Create_Should_SaveOrderToDatabase()
    {
        //Arrange
        var orderRepository = new OrderRepository(_productContext);
        var orderEntity = new Order { Orderdate = DateTime.Now };

        //Act
        var result = orderRepository.Create(orderEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void GetAll_Should_GetAllOrdersFromDatabase_ReturnIEnumerableOfTypeOrderEntity()
    {
        //Arrange
        var orderRepository = new OrderRepository(_productContext);
        var orderEntity = new Order { Orderdate = DateTime.Now };
        orderRepository.Create(orderEntity);

        //Act
        var result = orderRepository.GetAll();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<Order>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_GetOneOrderFromDatabase_ReturnOneOrderEntity()
    {
        //Arrange
        var orderRepository = new OrderRepository(_productContext);
        var orderEntity = new Order { Orderdate = DateTime.Now };
        orderRepository.Create(orderEntity);

        //Act
        var result = orderRepository.GetOne(x => x.Id == orderEntity.Id);

        //Assert
        Assert.Equal(orderEntity.Id, result.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneOrderFromDatabase_ReturnNull()
    {
        //Arrange
        var orderRepository = new OrderRepository(_productContext);
        var orderEntity = new Order { Orderdate = DateTime.Now };
        //orderRepository.Create(orderEntity);

        //Act
        var result = orderRepository.GetOne(x => x.Id == orderEntity.Id);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingOrder_ReturnUpdatedOrderEntity()
    {
        //Arrange
        var orderRepository = new OrderRepository(_productContext);
        var orderEntity = new Order { Orderdate = DateTime.Now };
        orderRepository.Create(orderEntity);

        //Act
        var updatedOrder = new DateTime(2024, 1, 13);
        var result = orderRepository.Update(x => x.Id == orderEntity.Id, orderEntity);

        //Assert
        Assert.Equal(orderEntity.Id, result.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void Delete_Should_DeleteOrderFromDatabase_ReturnTrue()
    {
        //Arrange
        var orderRepository = new OrderRepository(_productContext);
        var orderEntity = new Order { Orderdate = DateTime.Now };
        orderRepository.Create(orderEntity);

        //Act
        var result = orderRepository.Delete(x => x.Id == orderEntity.Id);

        //Assert
        Assert.True(result);
    }
}
