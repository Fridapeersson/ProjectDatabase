using DbProject.Contexts;
using DbProject.Entities;

namespace DbProject.Repositories;

public class RoleRepository : BaseRepository<RoleEntity, CustomerDbContext>
{
    public RoleRepository(CustomerDbContext context) : base(context)
    {
    }
}
