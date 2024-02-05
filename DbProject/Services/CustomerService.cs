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

    public CustomerEntity CreateCustomer(CreateCustomerDto customerDto)
    {
        try
        {
            if (!_customerRepository.Exists(x => x.Email == customerDto.Email))
            {
                var roleEntity = _roleService.GetOneRole(x => x.RoleName == customerDto.RoleName);
                var addressEntity = _addressService.GetOneAddress(x => x.Id == customerDto.Id);

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


    public bool DeleteCustomer(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            var customerEntity = _customerRepository.GetOne(predicate);
            if(customerEntity != null)
            {
                _customerRepository.Delete(x => x.Id ==  customerEntity.Id);
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
