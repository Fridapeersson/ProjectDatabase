using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class CustomerRepository_Tests
{
    private readonly CustomerDbContext _customerDbContext = new(new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void Create_Should_SaveOneCustomerEntityToDatabase()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com" };

        //Act
        var result = customerRepository.Create(customerEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, customerEntity.Id);
    }

    [Fact]
    public void Create_Should_Not_SaveOneCustomerEntityToDatabase_ReturnNull()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = null! };

        //Act
        var result = customerRepository.Create(customerEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_Should_GetAllCustomersFromDatabase_ReturnIEnumerableOfTypeCustomerEntity()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com" };
        customerRepository.Create(customerEntity);

        //Act
        var result = customerRepository.GetAll();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<CustomerEntity>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_GetOneCustomerFromDatabase_ReturnOneCustomer()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com", Role = roleEntity, Address = addressEntity };

        customerRepository.Create(customerEntity);

        //Act
        var result = customerRepository.GetOne(x => x.Id == customerEntity.Id);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(customerEntity.Id, result.Id);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneCustomerFromDatabase_ReturnNull()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        var addressEntity = new AddressEntity();
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com", Role = roleEntity, Address = addressEntity };

        customerRepository.Create(customerEntity);

        //Act
        var result = customerRepository.GetOne(x => x.Id == customerEntity.Id);

        //Assert
        Assert.Null(result);
    }


    [Fact]
    public void Update_Should_UpdateExistingCustomer_ReturnUpdatedCustomerEntity()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com", Role = roleEntity, Address = addressEntity };

        customerRepository.Create(customerEntity);

        //Act
        customerEntity.FirstName = "Testelina";
        var result = customerRepository.Update(x => x.Id == customerEntity.Id, customerEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(customerEntity.FirstName, result.FirstName);
        Assert.Equal("Testelina", customerEntity.FirstName);
    }

    [Fact]
    public void Update_Should_Not_UpdateExistingCustomer_ReturnNull()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com", Role = roleEntity, Address = addressEntity };

        //customerRepository.Create(customerEntity);

        //Act
        customerEntity.FirstName = "Testelina";
        var result = customerRepository.Update(x => x.Id == customerEntity.Id, customerEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_Should_DeleteCustomerFromDatabase_ReturnTrue()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com", Role = roleEntity, Address = addressEntity };

        customerRepository.Create(customerEntity);

        //Act
        var result = customerRepository.Delete(x => x.Id == customerEntity.Id);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_Should_Not_DeleteCustomerFromDatabase_ReturnFalse()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        var addressEntity = new AddressEntity { Street = "Test", PostalCode = "12345", City = "TestCity" };
        var customerEntity = new CustomerEntity { FirstName = "Test", LastName = "Testsson", Email = "Test@mail.com", Role = roleEntity, Address = addressEntity };

        //customerRepository.Create(customerEntity);

        //Act
        var result = customerRepository.Delete(x => x.Id == customerEntity.Id);

        //Assert
        Assert.False(result);
    }
}
