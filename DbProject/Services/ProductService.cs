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
