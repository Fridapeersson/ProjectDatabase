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

    /// <summary>
    ///     Creates a new addressEntity if it doesn't already exists in database
    /// </summary>
    /// <param name="entity">The address entity to create</param>
    /// <returns>The created address entity if it was successfully created, else null</returns>
    public AddressEntity CreateAddress(AddressEntity entity)
    {
        try
        {
            if (!_addressRepository.Exists(x => x.Id == entity.Id))
            {
                var addressEntity = _addressRepository.Create(new AddressEntity
                {
                    Id = entity.Id,
                    Street = entity.Street,
                    PostalCode = entity.PostalCode,
                    City = entity.City,
                });
                return addressEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one AddressEntity based on the provided predicate/expression
    /// </summary>
    /// <param name="predicate">The predicate/expression used to filter AddressEntity objects</param>
    /// <returns>The addressEntity that matches the predicate/expression, else null</returns>
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

    /// <summary>
    ///     Gets all AddressEntities from database
    /// </summary>
    /// <returns>A collection of AddressEntity objects, else null</returns>
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

    /// <summary>
    ///     Updates an existing AddressEntity in the database
    /// </summary>
    /// <param name="addressEntity">The addressEntity object containing updated information</param>
    /// <returns>The updated AddressEntity object, else null</returns>
    public AddressEntity UpdateAddress(AddressEntity addressEntity)
    {
        try
        {
            var updatedAddress = _addressRepository.Update(x => x.Id == addressEntity.Id, addressEntity);
            if (updatedAddress != null)
            {
                return updatedAddress;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
        }
        return null!;
    }

    /// <summary>
    ///     Deletes an addressEntity based on predicate/expression
    /// </summary>
    /// <param name="predicate">The predicate/expression used to filter address entities</param>
    /// <returns>True if deleted successfully, else false</returns>
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

    /// <summary>
    ///     Checks if the addressEntity with specified addressId has associated customers
    /// </summary>
    /// <param name="addressId">The id of the addressentity to check</param>
    /// <returns>True if the addressEntity has associated customers, else false</returns>
    //public bool HasCustomers(int addressId)
    //{
    //    try
    //    {
    //        //hömta kunder kopplade till addressen
    //        var customerInAddress = _customerRepository.GetAll().Where(x => x.AddressId == addressId);
    //        if(customerInAddress.Any())
    //        {
    //            return true;
    //        }
    //        //return customerInAddress.Any();
    //        return false;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("ERROR :: " + ex.Message);
    //        return false;
    //    }
    //}

}
