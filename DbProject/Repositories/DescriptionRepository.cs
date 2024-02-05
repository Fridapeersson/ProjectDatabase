using DbProject.Contexts;
using DbProject.Entities;

namespace DbProject.Repositories;

public class DescriptionRepository : BaseRepository<Description, ProductCatalogContext>
{
    public DescriptionRepository(ProductCatalogContext context) : base(context)
    {
    }
}
