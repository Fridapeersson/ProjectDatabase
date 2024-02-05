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
