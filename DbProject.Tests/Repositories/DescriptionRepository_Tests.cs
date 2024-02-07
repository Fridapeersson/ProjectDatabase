using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class DescriptionRepository_Tests
{
    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void Create_Should_SaveDescriptionToDatabase()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description { Ingress = "Test", DescriptionText = "Testing" };

        //Act
        var result = descriptionRepository.Create(descriptionEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void Create_Should_Not_SaveDescriptionToDatabase_ReturnNull()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description();

        //Act
        var result = descriptionRepository.Create(descriptionEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_Should_GetAllDescriptionsFromDatabase_ReturnIEnumerableOfTypeDescriptionEntity()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description();
        descriptionRepository.Create(descriptionEntity);

        //Act
        var result = descriptionRepository.GetAll();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<Description>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_GetOneDescriptionsFromDatabase_ReturnOneDescription()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description { Ingress = "Test", DescriptionText = "Testing" };
        descriptionRepository.Create(descriptionEntity);

        //Act
        var result = descriptionRepository.GetOne(x => x.Id == descriptionEntity.Id);

        //Assert
        Assert.Equal(descriptionEntity.Id, result.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneDescriptionsFromDatabase_ReturnNull()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description { Ingress = "Test", DescriptionText = "Testing" };
        //descriptionRepository.Create(descriptionEntity);

        //Act
        var result = descriptionRepository.GetOne(x => x.Id == descriptionEntity.Id);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingDescription_ReturnUpdatedDescriptionEntity()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description { Ingress = "Test", DescriptionText = "Testing" };
        descriptionRepository.Create(descriptionEntity);

        //Act
        descriptionEntity.Ingress = "TestTest";
        var result = descriptionRepository.Update(x => x.Id == descriptionEntity.Id, descriptionEntity);

        //Assert
        Assert.Equal(descriptionEntity.Ingress, result.Ingress);
        Assert.NotNull(result);
        Assert.Equal("TestTest", result.Ingress);
    }

    [Fact]
    public void Update_Should_Not_UpdateExistingDescription_ReturnNull()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description { Ingress = "Test", DescriptionText = "Testing" };
        //descriptionRepository.Create(descriptionEntity);

        //Act
        descriptionEntity.Ingress = "TestTest";
        var result = descriptionRepository.Update(x => x.Id == descriptionEntity.Id, descriptionEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_Should_DeleteDescriptionFromDatabase_ReturnTrue()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description { Ingress = "Test", DescriptionText = "Testing" };
        descriptionRepository.Create(descriptionEntity);

        //Act
        var result = descriptionRepository.Delete(x => x.Id == descriptionEntity.Id);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_Should_Not_DeleteDescriptionFromDatabase_ReturnFalse()
    {
        //Arrange
        var descriptionRepository = new DescriptionRepository(_productContext);
        var descriptionEntity = new Description { Ingress = "Test", DescriptionText = "Testing" };
        //descriptionRepository.Create(descriptionEntity);

        //Act
        var result = descriptionRepository.Delete(x => x.Id == descriptionEntity.Id);

        //Assert
        Assert.False(result);
    }
}
