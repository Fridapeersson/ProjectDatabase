using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Repositories;
using System.Linq.Expressions;

namespace DbProject.Services;

public class CustomerService
{
    private readonly CustomerRepository _customerRepository;
    private readonly AddressService _addressService;
    private readonly RoleService _roleService;

    public CustomerService(CustomerRepository customerRepository, AddressService addressService, RoleService roleService)
    {
        _customerRepository = customerRepository;
        _addressService = addressService;
        _roleService = roleService;
    }

    /// <summary>
    ///     Creates a new Customer in databse
    /// </summary>
    /// <param name="customerDto">the data transfer object containing the customer information</param>
    /// <returns>The created Customer entity, or null if the customer already exists.</returns>
    public CustomerEntity CreateCustomer(CreateCustomerDto customerDto)
    {
        try
        {
            if (!_customerRepository.Exists(x => x.Email == customerDto.Email))
            {
                var roleEntity = _roleService.GetOneRole(x => x.RoleName == customerDto.RoleName);
                var addressEntity = _addressService.GetOneAddress(x => x.Id == customerDto.Address.Id);

                if(roleEntity == null)
                {
                    roleEntity = new RoleEntity
                    {
                        RoleName = customerDto.RoleName,
                    };
                }
                if(addressEntity == null)
                {
                    addressEntity = new AddressEntity
                    {
                        Street = customerDto.Street,
                        PostalCode = customerDto.PostalCode,
                        City = customerDto.City,
                    };
                }

                var customerEntity = new CustomerEntity
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    Email = customerDto.Email,
                    Address = addressEntity,
                    Role = roleEntity
                };

                var createCustomer = _customerRepository.Create(customerEntity);
                if(createCustomer != null)
                {
                    return createCustomer;
                }
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets all customers from database
    /// </summary>
    /// <returns>A collection of all customers</returns>
    public IEnumerable<CustomerEntity> GetAllCustomers()
    {
        try
        {
            var customers = _customerRepository.GetAll();
            if(customers != null) 
            {
                var customerList = new HashSet<CustomerEntity>();
                foreach(var customer in customers)
                {
                    customerList.Add(customer);
                }
                return customerList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one customer from database based on the provided predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression used to filter the customer</param>
    /// <returns>the customer that matches the predicate/expression, else null</returns>
    public CustomerEntity GetOneCustomer(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            var customerEntity = _customerRepository.GetOne(predicate);

            if(customerEntity != null)
            {
                return customerEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Updates a customer in database
    /// </summary>
    /// <param name="entity">The updated customer entity</param>
    /// <returns>the updated customer entity</returns>
    public CustomerEntity UpdateCustomer(CustomerEntity entity)
    {
        try
        {
            var customerToUpdate = _customerRepository.Update(x => x.Id == entity.Id, entity);
            if(customerToUpdate != null)
            {
                return customerToUpdate;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes a customer entity from database based on the provided predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression used to filter the customer to delete</param>
    /// <returns>True if deleted successfully, else false</returns>
    public bool DeleteCustomer(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            var customerEntity = _customerRepository.GetOne(predicate);
            if(customerEntity != null)
            {
                var result = _customerRepository.Delete(x => x.Id ==  customerEntity.Id);
                if (result)
                {

                return true;
                }
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
