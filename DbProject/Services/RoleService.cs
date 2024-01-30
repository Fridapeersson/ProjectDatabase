using DbProject.Entities;
using DbProject.Repositories;
using System;
using System.Linq.Expressions;

namespace DbProject.Services;

public class RoleService
{
    private readonly RoleRepository _roleRepository;

    public RoleService(RoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public RoleEntity CreateRole(RoleEntity entity)
    {
        try
        {
            //if(!_roleRepository.Exists(x => x.RoleName == entity.RoleName))
            //{
                var roleEntity = _roleRepository.Create(new RoleEntity
                {
                    RoleName = entity.RoleName,
                });
            return roleEntity;
            //}
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


    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<< Kan vara shady >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
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
}
