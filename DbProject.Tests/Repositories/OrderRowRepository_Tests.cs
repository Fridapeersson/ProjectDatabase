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

    //Fixa
    [Fact]
    public void GetOne_Should_GetOneOrderRowFromDatabase_ReturnOneOrderRow()
    {
        //Arrange


        //Act

        //Assert
      
    }
}
