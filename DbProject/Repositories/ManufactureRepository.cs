using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class ManufactureRepository : BaseRepository<Manufacture, ProductCatalogContext>
{
    private readonly ProductCatalogContext _context;
    public ManufactureRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    ///     Gets all ManufactureEntity objects including associated objects (Products)
    /// </summary>
    /// <returns>A list of ManufactureEntities including associated objects (Products)</returns>
    public override IEnumerable<Manufacture> GetAll()
    {
        return _context.Manufactures
            .Include(i => i.Products)
            .ToList();
    }

    /// <summary>
    ///     Get one ManufactureEntity object with including associated objects Product) based on predicate/expression
    /// </summary>
    /// <param name="predicate">predicate/expression used to filter ManufactureEntitiy objects</param>
    /// <returns>An ManufactureEntity that matches the predicate/expression, including associated objects (Products)</returns>
    public override Manufacture GetOne(Expression<Func<Manufacture, bool>> predicate)
    {
        var manufactureEntity = _context.Manufactures
            .Include(i => i.Products)
            .FirstOrDefault(predicate);
        return manufactureEntity!;
    }
}
