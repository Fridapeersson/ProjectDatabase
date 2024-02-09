using ConsoleApp.Services;
using DbProject.Contexts;
using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var customerDbConnectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\frida\Desktop\Netutvecklare2023-2025\Datalagring\Project\DbProject\DbProject\Data\CustomerDb.mdf;Integrated Security=True;Connect Timeout=30";

var productCatalogConnectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\frida\Desktop\Netutvecklare2023-2025\Datalagring\Project\DbProject\DbProject\Data\ProductCatalog.mdf;Integrated Security=True;Connect Timeout=30";

var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<CustomerDbContext>(x => x.UseSqlServer(customerDbConnectionstring));
    services.AddDbContext<ProductCatalogContext>(x => x.UseSqlServer(productCatalogConnectionstring));


    //Customer
    services.AddScoped<AddressRepository>();
    services.AddScoped<CustomerRepository>();
    services.AddScoped<RoleRepository>();

    services.AddScoped<AddressService>();
    services.AddScoped<CustomerService>();
    services.AddScoped<RoleService>();


    //Products
    services.AddScoped<CategoryRepository>();
    services.AddScoped<DescriptionRepository>();
    services.AddScoped<ManufactureRepository>();
    services.AddScoped<OrderRepository>();
    services.AddScoped<OrderRowRepository>();
    services.AddScoped<ProductRepository>();
    services.AddScoped<ReviewRepository>();


    services.AddScoped<CategoryService>();
    services.AddScoped<DescriptionService>();
    services.AddScoped<ManufactureService>();
    services.AddScoped<OrderService>();
    services.AddScoped<OrderRowService>();
    services.AddScoped<ProductService>();
    services.AddScoped<ReviewService>();



    services.AddScoped<MenuService>();



}).Build();

var menuService = builder.Services.GetRequiredService<MenuService>();
menuService.StartMenu();