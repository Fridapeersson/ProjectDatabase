using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Services;

public class RoleService_Tests
{
    private readonly CustomerDbContext _customerDbContext = new(new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void CreateRole_Should_SaveRoleToDatabase()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleService = new RoleService(roleRepository);

        var roleEntity = new RoleEntity { RoleName = "Test" };

        //Act
        var result = roleService.CreateRole(roleEntity);

        //Assert
        Assert.Equal(1, result.Id);
        Assert.NotNull(result);
    }

    [Fact]
    public void CreateRole_Should_Not_SaveRoleToDatabase_ReturnNull()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleService = new RoleService(roleRepository);

        var roleEntity = new RoleEntity();

        //Act
        var result = roleService.CreateRole(roleEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAllRoles_Should_GetAllRoles_ReturnIEnumerableOfTypeRoleEntity()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleService = new RoleService(roleRepository);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        roleService.CreateRole(roleEntity);

        //Act
        var result = roleService.GetAllRoles();

        //Assert
        Assert.IsAssignableFrom<IEnumerable<RoleEntity>>(result);
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOneRole_Should_GetOneRole_ReturnOneRoleEntity()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleService = new RoleService(roleRepository);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        roleService.CreateRole(roleEntity);

        //Act
        var result = roleService.GetOneRole(x => x.RoleName == roleEntity.RoleName);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOneRole_Should_Not_GetOneRole_ReturnNull()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleService = new RoleService(roleRepository);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        //roleService.CreateRole(roleEntity);

        //Act
        var result = roleService.GetOneRole(x => x.RoleName == roleEntity.RoleName);

        //Assert
        Assert.Null(result);
    }

    //[Fact]
    //public void UpdateRole_Should_UpdateExistingRole_ReturnUpdatedRole()
    //{
    //    //Arrange
    //    var roleRepository = new RoleRepository(_customerDbContext);
    //    var roleService = new RoleService(roleRepository);

    //    var roleEntity = new RoleEntity { RoleName = "Test" };
    //    roleService.CreateRole(roleEntity);

    //    //Act
    //    roleEntity.RoleName = "Try";
    //    var result = roleService.UpdateRole(roleEntity);

    //    //Assert
    //    Assert.NotNull(result);
    //}

    [Fact]
    public void DeleteRole_Should_DeleteRoleEntity_ReturnTrue()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleService = new RoleService(roleRepository);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        roleService.CreateRole(roleEntity);

        //Act
        var result = roleService.DeleteRole(x => x.RoleName == roleEntity.RoleName);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void DeleteRole_Should_Not_DeleteRoleEntity_ReturnFalse()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleService = new RoleService(roleRepository);

        var roleEntity = new RoleEntity { RoleName = "Test" };
        //roleService.CreateRole(roleEntity);

        //Act
        var result = roleService.DeleteRole(x => x.RoleName == roleEntity.RoleName);

        //Assert
        Assert.False(result);
    }
}
