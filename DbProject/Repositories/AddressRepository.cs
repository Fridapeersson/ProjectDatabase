using DbProject.Contexts;
using DbProject.Entities;
using System.Linq.Expressions;

namespace DbProject.Repositories;

public class AddressRepository : BaseRepository<AddressEntity, CustomerDbContext>
{
    private readonly CustomerDbContext _customerContext;
    public AddressRepository(CustomerDbContext customerContext) : base(customerContext)
    {
        _customerContext = customerContext;
    }
}
