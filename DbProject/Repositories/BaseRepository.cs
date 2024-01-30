using DbProject.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class BaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    private readonly CustomerDbContext _customerContext;

    public BaseRepository(CustomerDbContext customerContext)
    {
        _customerContext = customerContext;
    }


    public virtual TEntity Create(TEntity entity)
    {
        try
        {
            _customerContext.Set<TEntity>().Add(entity);
            _customerContext.SaveChanges();
            return entity;
        }
        catch(Exception ex) {Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _customerContext.Set<TEntity>().FirstOrDefault(predicate);
            return entity!;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual IEnumerable<TEntity> GetAll()
    {
        try
        {
            var result = _customerContext.Set<TEntity>().ToList();
            return result;

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual TEntity Update(Expression<Func<TEntity, bool>> predicate, TEntity newEntity)
    {
        try
        {
            var existingEntity = _customerContext.Set<TEntity>().FirstOrDefault(predicate);
            if (existingEntity != null)
            {
                _customerContext.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                _customerContext.SaveChanges();
                return existingEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public virtual bool Delete(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _customerContext.Set<TEntity>().FirstOrDefault(predicate);
            if(entity != null)
            {
                _customerContext.Set<TEntity>().Remove(entity);
                _customerContext.SaveChanges();

                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var existing = _customerContext.Set<TEntity>().Any(predicate);
            return existing;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false!;
    }
}
