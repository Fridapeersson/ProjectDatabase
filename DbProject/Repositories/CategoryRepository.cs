using DbProject.Contexts;
using DbProject.Entities;

namespace DbProject.Repositories;

public class CategoryRepository : BaseRepository<Category, ProductCatalogContext>
{
    public CategoryRepository(ProductCatalogContext context) : base(context)
    {
    }
}
