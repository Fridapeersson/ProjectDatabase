using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class CustomerRepository : BaseRepository<CustomerEntity, CustomerDbContext>
{
    private readonly CustomerDbContext _customerContext;
    public CustomerRepository(CustomerDbContext customerContext) : base(customerContext)
    {
        _customerContext = customerContext;
    }

    public override bool Delete(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            var customerToDelete = _customerContext.Customer
            .Include(i => i.Address)
            .FirstOrDefault(predicate);

            if (customerToDelete != null)
            {
                _customerContext.Customer.Remove(customerToDelete);

                if(customerToDelete.Address != null)
                {
                    _customerContext.Address.Remove(customerToDelete.Address);
                }
                _customerContext.SaveChanges();
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }


    public override IEnumerable<CustomerEntity> GetAll()
    {
        try
        {
            return _customerContext.Customer
                .Include(i => i.Address)
                .Include(i => i.Role)
                .ToList();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override CustomerEntity GetOne(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            var entity = _customerContext.Customer
                .Include(i => i.Address)
                .Include(i => i.Role)
                .FirstOrDefault(predicate);

            return entity!;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
