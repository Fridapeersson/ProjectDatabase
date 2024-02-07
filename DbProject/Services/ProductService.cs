using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DbProject.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly ManufactureRepository _manufactureRepository;
    private readonly DescriptionRepository _descriptionRepository;

    public ProductService(ProductRepository productRepository, CategoryRepository categoryRepository, ManufactureRepository manufactureRepository, DescriptionRepository descriptionRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _manufactureRepository = manufactureRepository;
        _descriptionRepository = descriptionRepository;
    }

    /// <summary>
    ///     Creates a new product in the database using the provided DTO
    /// </summary>
    /// <param name="productDto">the data transfer object containing the information to create a new product</param>
    /// <returns>the new ProductEntity, else null if product already exists</returns>
    public Product CreateProduct(CreateProductDto productDto)
    {
        try
        {
            if(!_productRepository.Exists(x => x.Id == productDto.Id))
            {
                var categoryEntity = _categoryRepository.GetOne(x => x.CategoryName == productDto.CategoryName);
                var manufactureEntity = _manufactureRepository.GetOne(x => x.ManufactureName == productDto.ManufacureName);
                var descriptionEntity = _descriptionRepository.GetOne(x => x.Ingress ==  productDto.Ingress);

                if(categoryEntity == null)
                {
                    categoryEntity = new Category { CategoryName = productDto.CategoryName };
                }
                if(manufactureEntity == null)
                {
                    manufactureEntity = new Manufacture { ManufactureName = productDto.ManufacureName };
                }
                if(descriptionEntity == null)
                {
                    descriptionEntity = new Description
                    {
                        Ingress = productDto.Ingress,
                        DescriptionText = productDto.DescriptionText,
                    };
                }

                var productEntity = new Product
                {
                    ProductName = productDto.ProductName,
                    ProductPrice = productDto.ProductPrice,
                    Category = categoryEntity,
                    Manufacture = manufactureEntity,
                    Description = descriptionEntity
                };

                var createProduct = _productRepository.Create(productEntity);
                if(createProduct != null)
                {
                    return createProduct;
                }
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets all Products from database
    /// </summary>
    /// <returns>A collection of ProductEntity objects, else null</returns>
    public IEnumerable<Product> GetAllProducts()
    {
        try
        {
            var products = _productRepository.GetAll();
            if(products != null) 
            {
                var productList = new HashSet<Product>();
                foreach(var product in products)
                {
                    productList.Add(product);
                }
                return productList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one product entity from database based on the provided predicate/expression
    /// </summary>
    /// <param name="predicate">The predicate/expression used to filter ProductEntity objects</param>
    /// <returns>The ProducEntity object that matches the predicate/expression, else null</returns>
    public Product GetOneProduct(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            var productEntity = _productRepository.GetOne(predicate);
            if(productEntity != null)
            {
                return productEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Updates an existing ProductEntity
    /// </summary>
    /// <param name="productEntity">The productEntity object containing the updated data</param>
    /// <returns>The updated ProductEntity, else null</returns>
    public Product UpdateProduct(Product productEntity)
    {
        try
        {
            var productToUpdate = _productRepository.Update(x => x.Id == productEntity.Id, productEntity);
            if(productToUpdate != null)
            {
                return productToUpdate;
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes an ProductEntity based on provided predicate/expression
    /// </summary>
    /// <param name="predicate">The predicate/expression used to filter ProductEntity objects</param>
    /// <returns>True if deleted successfully, else false</returns>
    public bool DeleteProduct(Expression<Func<Product, bool>> predicate)
    {
        try
        {
            var productEntity = _productRepository.Delete(predicate);
            if(productEntity)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
