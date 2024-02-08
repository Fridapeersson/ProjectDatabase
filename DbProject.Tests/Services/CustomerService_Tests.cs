using DbProject.Contexts;
using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Services;

public class CustomerService_Tests
{
    private readonly CustomerDbContext _customerDbContext = new(new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void CreateCustomer_Should_SaveCustomerToDatabase()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);
        
        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            PostalCode = "12345"
        };

        //Act
        var result = customerService.CreateCustomer(customerDto);

        //Assert
        Assert.Equal(1, result.Id);
        Assert.NotNull(result);
        Assert.Equal("Test", customerDto.LastName);
    }

    [Fact]
    public void CreateCustomer_Should_Not_SaveCustomerToDatabase_RetunNull()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            //City = "Test",
            //PostalCode = "12345"
        };

        //Act
        var result = customerService.CreateCustomer(customerDto);

        //Assert
        Assert.Null(result);
    }


    [Fact]
    public void GetAllCustomers_Should_GetAllCustomersFromDatabase_ReturnIEnumerableOfTypeCustomerEntity()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            PostalCode = "12345"
        };
        customerService.CreateCustomer(customerDto);

        //Act
        var result = customerService.GetAllCustomers();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<CustomerEntity>>(result);
    }


    [Fact]
    public void GetOneCustomer_Should_GetOneCustomer_ReturnOneCustomerEntity()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            PostalCode = "12345"
        };
        customerService.CreateCustomer(customerDto);

        //Act
        var result = customerService.GetOneCustomer(x => x.Email == customerDto.Email);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void GetOneCustomer_Should_Not_GetOneCustomer_ReturnNull()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            //PostalCode = "12345"
        };
        customerService.CreateCustomer(customerDto);

        //Act
        var result = customerService.GetOneCustomer(x => x.Email == customerDto.Email);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateCustomer_Should_UpdateExistingCustomer_ReturnUpdatedCustomer()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            PostalCode = "12345"
        };
        customerService.CreateCustomer(customerDto);
        var customerEntity = customerService.GetOneCustomer(x => x.Email == customerDto.Email);


        //Act
        customerDto.Email = "T@test.com";
        var result = customerService.UpdateCustomer(customerEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void UpdateCustomer_Should_Not_UpdateExistingCustomer_ReturnNull()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            //PostalCode = "12345"
        };
        customerService.CreateCustomer(customerDto);
        var customerEntity = customerService.GetOneCustomer(x => x.Email == customerDto.Email);


        //Act
        customerDto.Email = "T@test.com";
        var result = customerService.UpdateCustomer(customerEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void DeleteCustomer_Should_DeleteCustomerFromDatabase_ReturnTrue()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            PostalCode = "12345"
        };
        customerService.CreateCustomer(customerDto);

        //Act
        var result = customerService.DeleteCustomer(x => x.Email == customerDto.Email);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void DeleteCustomer_Should_Not_DeleteCustomerFromDatabase_ReturnFalse()
    {
        //Arrange
        var customerRepository = new CustomerRepository(_customerDbContext);
        var addressRepository = new AddressRepository(_customerDbContext);
        var roleRepository = new RoleRepository(_customerDbContext);

        var roleService = new RoleService(roleRepository);
        var addressService = new AddressService(addressRepository);
        var customerService = new CustomerService(customerRepository, addressService, roleService);

        var customerDto = new CreateCustomerDto
        {
            FirstName = "Test",
            LastName = "Test",
            Email = "test@mail.com",
            RoleName = "Test",
            Street = "Test",
            City = "Test",
            PostalCode = "12345"
        };
        //customerService.CreateCustomer(customerDto);

        //Act
        var result = customerService.DeleteCustomer(x => x.Email == customerDto.Email);

        //Assert
        Assert.False(result);
    }
}
