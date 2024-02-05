using DbProject.Contexts;
using DbProject.Entities;

namespace DbProject.Repositories;

public class ManufactureRepository : BaseRepository<Manufacture, ProductCatalogContext>
{
    public ManufactureRepository(ProductCatalogContext context) : base(context)
    {
    }
}
