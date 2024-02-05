using DbProject.Contexts;
using DbProject.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class ProductRepository : BaseRepository<Product, ProductCatalogContext>
{
    private readonly ProductCatalogContext _context;

    public ProductRepository(ProductCatalogContext context) : base(context)
    {
        _context = context;
    }

    public override bool Delete(Expression<Func<Product, bool>> predicate)
    {
        var productToDelte = _context.Products
            .Include(i => i.Description)
            .FirstOrDefault(predicate);

        if(productToDelte != null)
        {
            _context.Products.Remove(productToDelte);
            if(productToDelte.Description != null)
            {
                _context.Descriptions.Remove(productToDelte.Description);
            }
            _context.SaveChanges();
            return true;
        }
        return false;
    }

    public override IEnumerable<Product> GetAll()
    {
        try
        {
            return _context.Products
                .Include(i => i.Manufacture)
                .Include(i => i.Category)
                .Include(i => i.Description)
                .Include(i => i.OrderRows)
                .Include(i => i.Reviews)
                .ToList();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override Product GetOne(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            var productEntity = _context.Products
                .Include(i => i.Manufacture)
                .Include(i => i.Category)
                .Include(i => i.Description)
                .Include(i => i.OrderRows)
                .Include(i => i.Reviews)
                .FirstOrDefault(predicate);

            return productEntity!;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

}
