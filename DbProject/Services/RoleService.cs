using DbProject.Entities;
using DbProject.Repositories;
using System;
using System.Linq.Expressions;

namespace DbProject.Services;

public class RoleService
{
    private readonly RoleRepository _roleRepository;
    private readonly CustomerRepository _customerRepository;


    public RoleService(RoleRepository roleRepository, CustomerRepository customerRepository)
    {
        _roleRepository = roleRepository;
        _customerRepository = customerRepository;
    }

    public RoleEntity CreateRole(RoleEntity entity)
    {
        try
        {
            if (!_roleRepository.Exists(x => x.RoleName == entity.RoleName))
            {
                var roleEntity = _roleRepository.Create(new RoleEntity
                {
                    RoleName = entity.RoleName,
                });
            return roleEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public RoleEntity GetOneRole(Expression<Func<RoleEntity, bool>> predicate)
    {
        try
        {
            var roleEntity = _roleRepository.GetOne(predicate);
            if (roleEntity != null)
            {
                return roleEntity;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }


    public IEnumerable<RoleEntity> GetAllRoles()
    {
        try
        {
            var roleEntity = _roleRepository.GetAll();
            if(roleEntity != null)
            {
                var roleList = new HashSet<RoleEntity>();
                foreach(var role in roleEntity)
                {
                    roleList.Add(role);
                }
                return roleList;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public RoleEntity UpdateRole(RoleEntity roleEntity)
    {
        try
        {
            var updatedRole = _roleRepository.Update(x => x.Id == roleEntity.Id, roleEntity);
            if(updatedRole != null)
            {
                return updatedRole;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    /// <summary>
    ///     Deletes a role based on the predicate/expression
    /// </summary>
    /// <param name="predicate">the predicate/expression to specify which role to delete</param>
    /// <returns>True if the role is deleted successfully, else false</returns>
    public bool DeleteRole(Expression<Func<RoleEntity, bool>> predicate)
    {
        try
        {
            var deleteRole = _roleRepository.Delete(predicate);
            if(deleteRole)
            {
                return true;
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    /// <summary>
    ///     checks if there is any customers associated with the specific role id
    /// </summary>
    /// <param name="roleId">the id of the role to check for associated customers</param>
    /// <returns>True if there are customers associated with the role id, else false</returns>
    public bool HasCustomers(int roleId)
    {
        try
        {
            //hämta kunder kopplade till rollen
            var customerInAddress = _customerRepository.GetAll().Where(x => x.RoleId == roleId);
            return customerInAddress.Any();

        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
    }
}
