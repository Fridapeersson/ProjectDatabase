using DbProject.Entities;
using DbProject.Repositories;
using System.Linq.Expressions;

namespace DbProject.Services;

public class DescriptionService
{
    private readonly DescriptionRepository _descriptionRepository;

    public DescriptionService(DescriptionRepository descriptionRepository)
    {
        _descriptionRepository = descriptionRepository;
    }

    /// <summary>
    ///     Creates a new description in the database
    /// </summary>
    /// <param name="entity">The descriptionEntity to create</param>
    /// <returns>the created descriptionEntity, else null</returns>
    public Description CreateDescription(Description entity)
    {
        try
        {
            var descriptionEntity = _descriptionRepository.Create(new Description
            {
                Ingress = entity.Ingress,
                DescriptionText = entity.DescriptionText,
            });
            return entity;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     gets all descriptions from database
    /// </summary>
    /// <returns>a collection of description objects, else null</returns>
    public IEnumerable<Description> GetAllDescriptions()
    {
        try
        {
            var descriptions = _descriptionRepository.GetAll();
            if(descriptions != null) 
            {
                var descriptionList = new HashSet<Description>();
                foreach(var description in descriptions)
                {
                    descriptionList.Add(description);
                }
                return descriptionList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one descriptionEntity based on provided predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression used to filter descriptionEntity objects</param>
    /// <returns>The DescriptionEntity that matches the predicate/expression, else null</returns>
    public Description GetOneDescription(Expression<Func<Description, bool>> predicate)
    {
        try
        {
            var descriptionEntity = _descriptionRepository.GetOne(predicate);
            if(descriptionEntity != null)
            {
                return descriptionEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Updates an existing DescriptionEntity 
    /// </summary>
    /// <param name="descriptionEntity">the descriptionEntity object containing updated data</param>
    /// <returns>The updated DescriptionEntity object, else null</returns>
    public Description UpdateDescription(Description descriptionEntity)
    {
        try
        {
            var updatedDescriptionEntity = _descriptionRepository.Update(x => x.Id == descriptionEntity.Id, descriptionEntity);
            if(updatedDescriptionEntity != null)
            {
                return updatedDescriptionEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes a DescriptionEntity from database based on the provided predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression used to filter Description entities </param>
    /// <returns>True if deleted successfully, else false</returns>
    public bool DeleteDescription(Expression<Func<Description, bool>> predicate)
    {
        try
        {
            var descriptionEntity = _descriptionRepository.Delete(predicate);
            if(descriptionEntity)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }
}
