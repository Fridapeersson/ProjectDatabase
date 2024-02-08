using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class AddressRepository_Tests
{
    private readonly CustomerDbContext _customerDbContext = new(new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);
    
    [Fact]
    public void Create_Should_SaveAddressEntityToDatabase()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Testgatan 3", PostalCode = "12345", City = "Helsingborg" };

        //Act
        var result = addressRepository.Create(addressEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void Update_Should_UpdateAddressEntityToDatabase()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Testgatan 3", PostalCode = "12345", City = "Helsingborg" };
        addressRepository.Create(addressEntity);

        //Act
        addressEntity.Street = "Test";
        var result = addressRepository.Update(x => x.Id == addressEntity.Id, addressEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal("Test", result.Street);
    }

    [Fact]
    public void Create_Should_Not_CreateNewAddressEntityToDatabase()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity();

        //Act
        var result = addressRepository.Create(addressEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
   public void GetAll_Should_GetAllAddressesFromDatabase_returnIEnumerableOfTypeAddressEntity()
   {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        addressRepository.Create(addressEntity);


        //Act
        var result = addressRepository.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
        Assert.Single(result);
   }

    [Fact]
    public void GetOne_Should_GetOneAddressFromDatabase_returnOneAddress()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        addressRepository.Create(addressEntity);

        //Act
        var result = addressRepository.GetOne(x => x.Street == addressEntity.Street);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(addressEntity.Street, result.Street);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneAddressFromDatabase_returnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };

        //Act
        var result = addressRepository.GetOne(x => x.Street == addressEntity.Street);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateExistingAddress_ReturnUpdatedAddressEntity()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        addressEntity = addressRepository.Create(addressEntity);

        //Act
        addressEntity.Street = "Testaregatan";
        var result = addressRepository.Update(x => x.Id == addressEntity.Id, addressEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(addressEntity.Id, result.Id);
        Assert.Equal("Testaregatan", result.Street);
    }

    [Fact]
    public void Update_Should_Not_UpdateExistingAddress_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };

        //Act
        addressEntity.Street = "Testaregatan";
        var result = addressRepository.Update(x => x.Id == addressEntity.Id, addressEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_Should_DeleteOneAddressEntity_ReturnTrue()
    {
        // Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        addressRepository.Create(addressEntity);

        // Act
        var result = addressRepository.Delete(x => x.Street == addressEntity.Street);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_Should_Not_DeleteAnAddressEntity_ReturnFalse()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };

        //Act
        var result = addressRepository.Delete(x => x.Street == addressEntity.Street);

        //Assert
        Assert.False(result);
    }
}
