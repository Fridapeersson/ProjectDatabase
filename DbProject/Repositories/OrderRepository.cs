using DbProject.Contexts;
using DbProject.Entities;

namespace DbProject.Repositories;

public class OrderRepository : BaseRepository<Order, ProductCatalogContext>
{
    public OrderRepository(ProductCatalogContext context) : base(context)
    {
    }
}
