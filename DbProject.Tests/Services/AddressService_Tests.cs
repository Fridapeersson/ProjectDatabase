using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Services;

public class AddressService_Tests
{
    private readonly CustomerDbContext _customerDbContext = new(new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void CreateAddress_Should_SaveAddressToDatabase()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressService = new AddressService(addressRepository);

        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };

        //Act
        var result = addressService.CreateAddress(addressEntity);

        //Assert
        Assert.Equal(1, result.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateAddress_Should_Not_SaveAddressToDatabase_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressService = new AddressService(addressRepository);

        var addressEntity = new AddressEntity();

        //Act
        var result = addressService.CreateAddress(addressEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAllAddresses_Should_GetAllAddressesFromDatabase_ReturnIEnumerableOfTypeAddressEntity()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressService = new AddressService(addressRepository);

        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        addressService.CreateAddress(addressEntity);

        //Act
        var result = addressService.GetAllAddresses();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<AddressEntity>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOneAddress_Should_GetOneAddressEntity_ReturnOneAddressEntity()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressService = new AddressService(addressRepository);

        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        addressService.CreateAddress(addressEntity);

        //Act
        var result = addressService.GetOneAddress(x => x.Street == addressEntity.Street);

        //Assert
        Assert.Equal("Test", addressEntity.Street);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOneAddress_Should_Not_GetOneAddressEntity_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressService = new AddressService(addressRepository);

        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        //addressService.CreateAddress(addressEntity);

        //Act
        var result = addressService.GetOneAddress(x => x.Street == addressEntity.Street);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void DeleteAddress_Should_DeleteAddressFromDatabase_ReturnTrue()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressService = new AddressService(addressRepository);

        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        addressService.CreateAddress(addressEntity);

        //Act
        var result = addressService.DeleteAddress(x => x.Street == addressEntity.Street);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void DeleteAddress_Should_Not_DeleteAddressFromDatabase_ReturnFalse()
    {
        //Arrange
        var addressRepository = new AddressRepository(_customerDbContext);
        var addressService = new AddressService(addressRepository);

        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        //addressService.CreateAddress(addressEntity);

        //Act
        var result = addressService.DeleteAddress(x => x.Street == addressEntity.Street);

        //Assert
        Assert.False(result);
    }
}
