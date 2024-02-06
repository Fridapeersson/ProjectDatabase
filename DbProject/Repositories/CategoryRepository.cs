using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class CategoryRepository : BaseRepository<Category, ProductCatalogContext>
{
    private readonly ProductCatalogContext _context;
    public CategoryRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Gets all categoryEntity objects including associated objects (products)
    /// </summary>
    /// <returns>A list of categoryEntities including associated objects (products)</returns>
    public override IEnumerable<Category> GetAll()
    {
        try
        {
            return _context.Categories
                .Include(i => i.Products)
                .ToList();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;

    }

    /// <summary>
    ///     Get one categoryEntity object with including associated objects (products) based on predicate/expression
    /// </summary>
    /// <param name="predicate">predicate/expression used to filter categoryEntitiy objects</param>
    /// <returns>An categoryEntity that matches the predicate/expression, including associated objects (products)</returns>
    public override Category GetOne(Expression<Func<Category, bool>> predicate)
    {
        try
        {
            var categoryEntity = _context.Categories
                .Include(i => i.Products)
                .FirstOrDefault(predicate);

            if (categoryEntity != null)
            {
                return categoryEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}
