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
