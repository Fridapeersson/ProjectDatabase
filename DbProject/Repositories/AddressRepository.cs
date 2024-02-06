using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class AddressRepository : BaseRepository<AddressEntity, CustomerDbContext>
{
    private readonly CustomerDbContext _context;
    public AddressRepository(CustomerDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Gets all addressEntities including associated objects (customer)
    /// </summary>
    /// <returns>A list of all addressentities including associated objects (customers)</returns>
    public override IEnumerable<AddressEntity> GetAll()
    {
        return _context.Address
            .Include(i => i.Customers)
            .ToList();
    }

    /// <summary>
    ///     Gets one addressentity based on predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression used to filter addressentity objects</param>
    /// <returns>An addressentity that matches the predicate/expression, including associated objects (customers)</returns>
    public override AddressEntity GetOne(Expression<Func<AddressEntity, bool>> predicate)
    {
        var addressEntity = _context.Address
            .Include(i => i.Customers)
            .FirstOrDefault(predicate);

        return addressEntity!;
    }
}
