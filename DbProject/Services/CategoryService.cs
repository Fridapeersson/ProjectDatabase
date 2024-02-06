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

    /// <summary>
    ///     Creates a new CategoryEntity in database
    /// </summary>
    /// <param name="entity">the category entity to create</param>
    /// <returns>the created categoryentity</returns>
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
    /// <summary>
    ///     Gets all categoryEntities from database
    /// </summary>
    /// <returns>A collection of all categories</returns>
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

    /// <summary>
    ///     Gets one categoryentity from database based on provided predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression that filter the category</param>
    /// <returns>the categoryEntity that matches the predicate/expression</returns>
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

    /// <summary>
    ///     Updates a category in database
    /// </summary>
    /// <param name="categoryEntity">The updated category entity</param>
    /// <returns>the updated CategoryEntity</returns>
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

    /// <summary>
    ///     Deletes a category from database based on the provided predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression that filter the categories to delete</param>
    /// <returns>True if deleted successfully, else false</returns>
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

    /// <summary>
    ///     Checks if there are any products associated wioth the specified category
    /// </summary>
    /// <param name="categoryId">the if of the category to check for associated products</param>
    /// <returns>True if there is associated products, else false</returns>
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
