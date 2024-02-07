using DbProject.Contexts;
using DbProject.Entities;
using DbProject.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Tests.Repositories;

public class BaseRepository_Tests
{
    private readonly CustomerDbContext _customerDbContext = new(new DbContextOptionsBuilder<CustomerDbContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);

    private readonly ProductCatalogContext _productContext = new(new DbContextOptionsBuilder<ProductCatalogContext>().UseInMemoryDatabase($"{Guid.NewGuid()}").Options);




}
