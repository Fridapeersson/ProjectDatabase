using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class RoleRepository : BaseRepository<RoleEntity, CustomerDbContext>
{
    private readonly CustomerDbContext _context;
    public RoleRepository(CustomerDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Gets allRoleEntity objects including associated objects (Customers)
    /// </summary>
    /// <returns>A list of RoleEntities including associated objects (Customers)</returns>
    public override IEnumerable<RoleEntity> GetAll()
    {
        try
        {
            return _context.Roles
                .Include(i => i.Customers)
                .ToList();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;

    }

    /// <summary>
    ///     Get one RoleEntity object with including associated objects (Customers) based on predicate/expression
    /// </summary>
    /// <param name="predicate">predicate/expression used to filter RoleEntitiy objects</param>
    /// <returns>An RoleEntity that matches the predicate/expression, including associated objects (Customers)</returns>
    public override RoleEntity GetOne(Expression<Func<RoleEntity, bool>> predicate)
    {
        try
        {
            var roleEntity = _context.Roles
                .Include(i => i.Customers)
                .FirstOrDefault(predicate);

            return roleEntity!;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;

    }
}
