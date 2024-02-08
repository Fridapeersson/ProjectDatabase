using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Services;

public class DescriptionService_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void CreateCategory_Should_SaveCategoryToDatabase()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptioService = new DescriptionService(descriptionRepository);
        var descriptioEntity = new Description { Ingress = "Test", DescriptionText = "Test" };

        //Act
        var result = descriptioService.CreateDescription(descriptioEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Ingress);
    }


}
