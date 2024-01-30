using DbProject.Entities;
using DbProject.Repositories;
using System.Linq.Expressions;

namespace DbProject.Services;

public class AddressService
{
    private readonly AddressRepository _addressRepository;

    public AddressService(AddressRepository addressRepository)
    {
        _addressRepository = addressRepository;
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
            var addressEntity = _addressRepository.GetAll();
            if(addressEntity != null)
            {
                var addressList = new HashSet<AddressEntity>();
                foreach(var address in addressEntity)
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

}
