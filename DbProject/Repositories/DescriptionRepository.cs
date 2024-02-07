using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class DescriptionRepository : BaseRepository<Description, ProductCatalogContext>
{
    private readonly ProductCatalogContext _context;
    public DescriptionRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Gets all Descriptions from database including associated Products
    /// </summary>
    /// <returns>A collection of DescriptionEntity objects with associated Products</returns>
    public override IEnumerable<Description> GetAll()
    {
        try
        {
            return _context.Descriptions
                .Include(i => i.Products)
                .ToList();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;

    }
}
