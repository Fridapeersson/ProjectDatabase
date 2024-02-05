using DbProject.Entities;
using DbProject.Repositories;
using System.Linq.Expressions;

namespace DbProject.Services;

public class AddressService
{
    private readonly AddressRepository _addressRepository;
    private readonly ProductRepository _productRepository;
    private readonly CustomerRepository _customerRepository;

    public AddressService(AddressRepository addressRepository, ProductRepository productRepository, CustomerRepository customerRepository)
    {
        _addressRepository = addressRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
    }

    public AddressEntity CreateAddress(AddressEntity entity)
    {
        try
        {
            //if(!_addressRepository.Exists(x => x.Id == entity.Id))
            //{
                var addressEntity = _addressRepository.Create(new AddressEntity
                {
                    Id = entity.Id,
                    Street = entity.Street,
                    PostalCode = entity.PostalCode,
                    City = entity.City,
                });
                return addressEntity;
            //}
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public AddressEntity GetOneAddress(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            var addressEntity = _addressRepository.GetOne(predicate);
            if(addressEntity != null)
            {
                return addressEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public IEnumerable<AddressEntity> GetAllAddresses()
    {
        try
        {
            var addresses = _addressRepository.GetAll();
            if(addresses != null)
            {
                var addressList = new HashSet<AddressEntity>();
                foreach(var address in addresses)
                {
                    addressList.Add(address);
                }
                return addressList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public AddressEntity UpdateAddress(AddressEntity addressEntity)
    {
        try
        {
            var updatedAddressEntity = _addressRepository.Update(x => x.Id == addressEntity.Id, addressEntity);
            if(updatedAddressEntity != null)
            {
                return updatedAddressEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public bool DeleteAddress(Expression<Func<AddressEntity, bool>> predicate)
    {
        try
        {
            var result = _addressRepository.Delete(predicate);
            if(result)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    public bool HasCustomers(int addressId)
    {
        try
        {
            //hömta kunder kopplade till addressen
            var customerInAddress = _customerRepository.GetAll().Where(x => x.Id == addressId);
            return customerInAddress.Any();

        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
    }

}
