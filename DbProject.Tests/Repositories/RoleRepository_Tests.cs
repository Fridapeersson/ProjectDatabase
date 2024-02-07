using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class RoleRepository_Tests
{
    private readonly CustomerDbContext _customerDbContext = new(new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    [Fact]
    public void Create_Should_SaveRoleEntityToDatabase()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity { RoleName = "Test" };

        //Act
        var result = roleRepository.Create(roleEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void Create_Should_Not_SaveRoleEntityToDatabase_ReturnNull()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity();

        //Act
        var result = roleRepository.Create(roleEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAll_Should_GetAllRoles_ReturnIEnumerableOfTypeRoleEntity()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity { RoleName = "Test" };
        roleRepository.Create(roleEntity);

        //Act
        var result = roleRepository.GetAll();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<RoleEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public void GetOne_Should_GetOneRole_ReturnOneRole()
    {
        //Assert
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity { RoleName = "Test" };
        roleRepository.Create(roleEntity);

        //Act
        var result = roleRepository.GetOne(x => x.RoleName == roleEntity.RoleName);

        //Arrange
        Assert.NotNull(result);
    }

    [Fact]
    public void GetOne_Should_Not_GetOneRole_ReturnNull()
    {
        //Assert
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity { RoleName = "Test" };

        //Act
        var result = roleRepository.GetOne(x => x.RoleName == roleEntity.RoleName);

        //Arrange
        Assert.Null(result);
    }

    [Fact]
    public void Update_Should_UpdateAnExistingRole_ReturnUpdatedRoleEntity()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity { RoleName = "Test" };
        roleRepository.Create(roleEntity);

        //Act
        roleEntity.RoleName = "Testarrollen";
        var result = roleRepository.Update(x => x.Id == roleEntity.Id ,roleEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(roleEntity.Id, result.Id);
        Assert.Equal("Testarrollen", roleEntity.RoleName);
    }


    [Fact]
    public void Update_Should_Not_UpdateAnExistingRole_ReturnNull()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity {  RoleName = "Test" };

        //Act
        roleEntity.RoleName = "Testarrollen";
        var result = roleRepository.Update(x => x.Id == roleEntity.Id, roleEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_Should_DeleteOneRoleEntity_ReturnTrue()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity { RoleName = "Test" };
        roleRepository.Create(roleEntity);

        //Act
        var result = roleRepository.Delete(x => x.RoleName == roleEntity.RoleName);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public void Delete_Should_Not_DeleteOneRoleEntity_ReturnFalse()
    {
        //Arrange
        var roleRepository = new RoleRepository(_customerDbContext);
        var roleEntity = new RoleEntity { RoleName = "Test" };

        //Act
        var result = roleRepository.Delete(x => x.RoleName == roleEntity.RoleName);

        //Assert
        Assert.False(result);
    }
}
