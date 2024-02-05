using DbProject.Entities;
using DbProject.Repositories;
using System.Linq.Expressions;

namespace DbProject.Services;

public class CategoryService
{
    private readonly CategoryRepository _categoryRepository;
    private readonly ProductRepository _productRepository;


    public CategoryService(CategoryRepository categoryRepository, ProductRepository productRepository)
    {
        _categoryRepository = categoryRepository;
        _productRepository = productRepository;
    }


    public Category CreateCategory(Category entity)
    {
        try
        {
            var categoryEntity = _categoryRepository.Create(new Category
            {
                CategoryName = entity.CategoryName
            });
            return categoryEntity;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public IEnumerable<Category> GetAllCategories()
    {
        try
        {
            var categories = _categoryRepository.GetAll();
            if(categories != null)
            {
                var categoryList = new HashSet<Category>();
                foreach (var category in categories)
                {
                    categoryList.Add(category);
                }
                return categoryList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public Category GetOneCategory(Expression<Func<Category, bool>> predicate)
    {
        try
        {
            var categoryEntity = _categoryRepository.GetOne(predicate);
            if(categoryEntity != null)
            {
                return categoryEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public Category UpdateCategory(Category categoryEntity)
    {
        try
        {
            var updatedCategoryEntity = _categoryRepository.Update(x => x.Id == categoryEntity.Id, categoryEntity);
            if(updatedCategoryEntity != null)
            {
                return updatedCategoryEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public bool DeleteCategory(Expression<Func<Category, bool>> predicate)
    {
        try
        {
            var categoryEntity = _categoryRepository.Delete(predicate);
            if(categoryEntity)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    public bool HasProducts(int categoryId)
    {
        try
        {
           //hämta produkter kopplade till kategorin
            var productsInCategory = _productRepository.GetAll().Where(p => p.CategoryId == categoryId);

            //kollar om det finns några kategorier i listan
            return productsInCategory.Any();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
    }
}
