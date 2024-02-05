using DbProject.Contexts;
using DbProject.Entities;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class AddressRepository : BaseRepository<AddressEntity, CustomerDbContext>
{
    public AddressRepository(CustomerDbContext context) : base(context)
    {
    }
}
