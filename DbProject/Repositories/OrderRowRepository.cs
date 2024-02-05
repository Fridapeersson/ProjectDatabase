using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class OrderRowRepository : BaseRepository<OrderRow, ProductCatalogContext>
{
    private readonly ProductCatalogContext _context;
    public OrderRowRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Deletes an orderrow and its associated order from database based on predicate/expression
    /// </summary>
    /// <param name="predicate">to locate the orderrow to be deleted</param>
    /// <returns>True if orderrow and order has been deleted successfully, else false</returns>
    public override bool Delete(Expression<Func<OrderRow, bool>> predicate)
    {
        try
        {
            var orderRowToDelete = _context.OrderRows
                .Include(i => i.Order)
                .FirstOrDefault(predicate);

            if (orderRowToDelete != null)
            {
                _context.OrderRows.Remove(orderRowToDelete);
                if (orderRowToDelete.Order != null)
                {
                    _context.Orders.Remove(orderRowToDelete.Order);
                    
                }
            }
            _context.SaveChanges();
            return true;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;  
    }

    /// <summary>
    ///     Gets all orderrows from database including associated order and product
    /// </summary>
    /// <returns>An Enumerable collection of all orderrows with associated orders and products</returns>
    public override IEnumerable<OrderRow> GetAll()
    {
        return _context.OrderRows
            .Include(i => i.Order)
            .Include(i => i.Product)
            .ToList();
    }

    /// <summary>
    ///     Gets one order row from database with associated order and product, based on the predicate/expression
    /// </summary>
    /// <param name="predicate">to locate specific orderrow</param>
    /// <returns>The order row with associated product and order that matches the predicate/expression</returns>
    public override OrderRow GetOne(Expression<Func<OrderRow, bool>> predicate)
    {
        var orderRowEntity = _context.OrderRows
            .Include(i => i.Order)
            .Include(i => i.Product)
            .FirstOrDefault(predicate);

        return orderRowEntity!;
    }
}
