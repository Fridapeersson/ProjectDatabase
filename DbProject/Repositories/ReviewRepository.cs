using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class ReviewRepository : BaseRepository<Review, ProductCatalogContext>
{
    private readonly ProductCatalogContext _context;
    public ReviewRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Gets all ReviewEntity objects including associated objects (Product)
    /// </summary>
    /// <returns>A list of ReviewEntities including associated objects (Product)</returns>
    public override IEnumerable<Review> GetAll()
    {
        return _context.Reviews
            .Include(i => i.Product)
            .ToList();
    }

    /// <summary>
    ///     Get one ReviewEntity object with including associated objects (Product) based on predicate/expression
    /// </summary>
    /// <param name="predicate">predicate/expression used to filter ReviewEntitiy objects</param>
    /// <returns>An ReviewEntity that matches the predicate/expression, including associated objects (Product)</returns>
    public override Review GetOne(Expression<Func<Review, bool>> predicate)
    {
        try
        {
            var reviewEntity = _context.Reviews
                .Include(i => i.Product)
                .FirstOrDefault(predicate);

            return reviewEntity!;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
