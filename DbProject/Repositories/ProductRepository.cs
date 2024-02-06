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

/// <summary>
///     deletes a product based on the preducate/expression
/// </summary>
/// <param name="predicate">the predicate/expression used to filter the product to be deleted</param>
/// <returns>True if the product was deleted, else false</returns>
    public override bool Delete(Expression<Func<Product, bool>> predicate)
    {
        var productToDelete = _context.Products
            .Include(i => i.Description)
            .Include(i => i.Reviews)
            .FirstOrDefault(predicate);

        if(productToDelete != null)
        {
            _context.Reviews.RemoveRange(productToDelete.Reviews);

            _context.Products.Remove(productToDelete);
            if(productToDelete.Description != null)
            {
                _context.Descriptions.Remove(productToDelete.Description);
            }

            _context.SaveChanges();
            return true;
        }
        return false;
    }

    /// <summary>
    ///     Gets all ProductEntity objects including associated objects (Manufacture, Category, Description, OrderRows, Reviews)
    /// </summary>
    /// <returns>A list of ProductEntities including associated objects (Manufacture, Category, Description, OrderRows, Reviews)</returns>
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

    /// /// <summary>
    ///     Get one ProductEntity object with including associated objects (Manufacture, Category, Description, OrderRows, Reviews) based on predicate/expression
    /// </summary>
    /// <param name="predicate">predicate/expression used to filter ProductEntitiy objects</param>
    /// <returns>An ProductEntity that matches the predicate/expression, including associated objects (Manufacture, Category, Description, OrderRows, Reviews)</returns>

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
