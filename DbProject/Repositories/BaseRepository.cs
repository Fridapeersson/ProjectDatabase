using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class BaseRepository<TEntity, TContext> where TEntity : class where TContext : DbContext
{
    private readonly TContext _context;

    public BaseRepository(TContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Saves an entity to database
    /// </summary>
    /// <param name="entity">The entity to be saved</param>
    /// <returns>the saved entity</returns>
    public virtual TEntity Create(TEntity entity)
    {
        try
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }
        catch(Exception ex) {Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets one entity from database based on the predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression to filter the entities</param>
    /// <returns>the entity that matches predicate/expression, else returns null</returns>
    public virtual TEntity GetOne(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(predicate);
            return entity!;
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Gets all entities from database
    /// </summary>
    /// <returns>a list of all entities</returns>
    public virtual IEnumerable<TEntity> GetAll()
    {
        try
        {
            var result = _context.Set<TEntity>().ToList();
            return result;

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Updates an entity in database based on the specific predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression to find the entity to be updated</param>
    /// <param name="newEntity">the new entity data to update</param>
    /// <returns>the updated entity if found, else returns null</returns>
    public virtual TEntity Update(Expression<Func<TEntity, bool>> predicate, TEntity newEntity)
    {
        try
        {
            var existingEntity = _context.Set<TEntity>().FirstOrDefault(predicate);
            if (existingEntity != null)
            {
                _context.Entry(existingEntity).CurrentValues.SetValues(newEntity);
                _context.SaveChanges();
                return existingEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes an entity based on the specific predicate/expression
    /// </summary>
    /// <param name="predicate">>the predicate/expression to find the entity to be deleted</param>
    /// <returns>true if deleted successfully, else false</returns>
    public virtual bool Delete(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(predicate);
            if(entity != null)
            {
                _context.Set<TEntity>().Remove(entity);
                _context.SaveChanges();

                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    /// <summary>
    ///     checks based on the predicate/expression if an entity exists
    /// </summary>
    /// <param name="predicate">the predicate/expression to check if entity exists</param>
    /// <returns>True if it exists, else false</returns>
    public virtual bool Exists(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var existing = _context.Set<TEntity>().Any(predicate);
            return existing;

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false!;
    }

    public bool HasCustomers(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var customerInEntity = _context.Set<TEntity>().Any(predicate);
            return customerInEntity;
            //hömta kunder kopplade till addressen
            //var customerInAddress = _customerService.GetAllCustomers().Where(predicate);
            //if (customerInAddress.Any())
            //{
            //    return true;
            //}
            //return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
    }
}
