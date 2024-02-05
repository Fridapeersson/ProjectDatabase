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

    public override IEnumerable<Review> GetAll()
    {
        return _context.Reviews
            .Include(i => i.Product)
            .ToList();
    }

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
