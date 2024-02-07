using DbProject.Entities;
using DbProject.Repositories;
using System.Linq.Expressions;

namespace DbProject.Services;

public class ManufactureService
{
    private readonly ManufactureRepository _manufactureRepository;
    private readonly ProductRepository _productRepository;


    public ManufactureService(ManufactureRepository manufactureRepository, ProductRepository productRepository)
    {
        _manufactureRepository = manufactureRepository;
        _productRepository = productRepository;
    }

    /// <summary>
    ///     Creates a ManufactureEntity to database
    /// </summary>
    /// <param name="entity">The manufacture entity to be created</param>
    /// <returns>the created ManufactureEntity, else null</returns>
    public Manufacture CreateManufacture(Manufacture entity)
    {
        try
        {
            var manufactureEntity = _manufactureRepository.Create(new Manufacture
            {
                ManufactureName = entity.ManufactureName
            });
            return manufactureEntity;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets all Manufactures from database
    /// </summary>
    /// <returns>a collection of Manufacture objects, else null</returns>
    public IEnumerable<Manufacture> GetAllManufactures()
    {
        try
        {
            var manufactures = _manufactureRepository.GetAll();
            if(manufactures != null)
            {
                var manufactureList = new HashSet<Manufacture>();
                foreach(var manufacture in manufactures)
                {
                    manufactureList.Add(manufacture);
                }
                return manufactureList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one ManufactureEntity based on provided predicate/expression
    /// </summary>
    /// <param name="expression">The predicate/expression used to filter ManufactureEntity objects</param>
    /// <returns>The ManufactureEntity that matches the predicate/expression</returns>
    public Manufacture GetOneManufacture(Expression<Func<Manufacture, bool>> expression)
    {
        try
        {
            var manufactureEntity = _manufactureRepository.GetOne(expression);
            if(manufactureEntity != null)
            {
                return manufactureEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Updates an existing ManufactureEntity
    /// </summary>
    /// <param name="manufactureEntity">The manufactureEntity containing the updated data</param>
    /// <returns>The updated ManufactureEntity, else null</returns>
    public Manufacture UpdateManufacture(Manufacture manufactureEntity)
    {
        try
        {
            var updatedManufacture = _manufactureRepository.Update(x => x.Id == manufactureEntity.Id, manufactureEntity);
            if(updatedManufacture != null)
            {
                return updatedManufacture;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes a ManufactureEntity based on provided predicate/expression
    /// </summary>
    /// <param name="expression">The predicate/expression used to filter ManufactureEntity objects</param>
    /// <returns>True if deleted successfully, else false</returns>
    public bool DeleteManufacture(Expression<Func<Manufacture, bool>> expression)
    {
        try
        {
            var manufactureEntity = _manufactureRepository.Delete(expression);
            if(manufactureEntity)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    /// <summary>
    ///     Checks if there are any products associated with the specified manufactureId
    /// </summary>
    /// <param name="manufactureId">The id of the manufacture to check for associated products</param>
    /// <returns>True if there is associated products, else false</returns>
    public bool HasProducts(int manufactureId)
    {
        try
        {
            //hämta produkter kopplade till tillverkaren
            var productsInManufacture = _productRepository.GetAll().Where(x => x.ManufactureId == manufactureId);

            //kollar om det finns några tillverkare i listan
            return productsInManufacture.Any();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
    }
}