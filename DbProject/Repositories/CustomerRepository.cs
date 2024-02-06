using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class CustomerRepository : BaseRepository<CustomerEntity, CustomerDbContext>
{
    private readonly CustomerDbContext _customerContext;
    public CustomerRepository(CustomerDbContext context) : base(context)
    {
        _customerContext = context;
    }
    /// <summary>
    ///     Gets all CustomerEntity objects including associated objects (Address, Role)
    /// </summary>
    /// <returns>A list of CustomerEntities including associated objects (Address, Role)</returns>
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

    /// <summary>
    ///     Get one CustomerEntity object with including associated objects (Address, Role) based on predicate/expression
    /// </summary>
    /// <param name="predicate">predicate/expression used to filter CustomerEntitiy objects</param>
    /// <returns>An CustomerEntity that matches the predicate/expression, including associated objects (Address, Role)</returns>
    public override CustomerEntity GetOne(Expression<Func<CustomerEntity, bool>> predicate)
    {
        try
        {
            var customerEntity = _customerContext.Customer
                .Include(i => i.Address)
                .Include(i => i.Role)
                .FirstOrDefault(predicate);

            return customerEntity!;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
