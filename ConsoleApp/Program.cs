using ConsoleApp.Services;
using DbProject.Contexts;
using DbProject.Dtos;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var customerDbConnectionstring = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\frida\Desktop\Netutvecklare2023-2025\Datalagring\Project\DbProject\DbProject\Data\CustomerDb.mdf;Integrated Security=True;Connect Timeout=30";
var builder = Host.CreateDefaultBuilder().ConfigureServices(services =>
{
    services.AddDbContext<CustomerDbContext>(x => x.UseSqlServer(customerDbConnectionstring));

    services.AddSingleton<MenuService>();

    services.AddSingleton<AddressRepository>();
    services.AddSingleton<CustomerRepository>();
    services.AddSingleton<RoleRepository>();

    services.AddSingleton<AddressService>();
    services.AddSingleton<CustomerService>();
    services.AddSingleton<RoleService>();



}).Build();


var menuService = builder.Services.GetService<MenuService>();

menuService.StartMenu();

//var testCustomer = new CreateCustomerDto
//{
//    FirstName = "Frida",
//    LastName = "Testsson",
//    Email = "Frida@test.test",
//    RoleName = "User",
//    Street = "Gatan 12",
//    PostalCode = "123 45",
//    City = "Testbyn",

//};

//var createdCustomer = customerService.CreateCustomer(testCustomer);
//if (createdCustomer)
//{
//    Console.WriteLine("Customer created successfully!");
//}
//else
//{
//    Console.WriteLine("Failed to create customer.");
//}