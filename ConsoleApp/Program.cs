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
    services.AddSingleton<AddressRepository>();
    services.AddSingleton<CustomerRepository>();
    services.AddSingleton<RoleRepository>();

    services.AddSingleton<AddressService>();
    services.AddSingleton<CustomerService>();
    services.AddSingleton<RoleService>();


    //Products
    services.AddSingleton<CategoryRepository>();
    services.AddSingleton<DescriptionRepository>();
    services.AddSingleton<ManufactureRepository>();
    services.AddSingleton<OrderRepository>();
    services.AddSingleton<OrderRowRepository>();
    services.AddSingleton<ProductRepository>();
    services.AddSingleton<ReviewRepository>();


    services.AddSingleton<CategoryService>();
    services.AddSingleton<DescriptionService>();
    services.AddSingleton<ManufactureService>();
    services.AddSingleton<OrderService>();
    services.AddSingleton<OrderRowService>();
    services.AddSingleton<ProductService>();
    services.AddSingleton<ReviewService>();



    services.AddSingleton<MenuService>();



}).Build();

var menuService = builder.Services.GetRequiredService<MenuService>();

//menuService.StartMenu();
menuService.StartMenu();


//var productService = builder.Services.GetRequiredService<ProductService>();
//var addProduct = new CreateProductDto
//{
//    ProductName = "Samsung galaxy s24",
//    ProductPrice = 12345,
//    DescriptionText = "Description",
//    Ingress = "Ingress",
//    CategoryName = "Category",
//    ManufacureName = "Manufacture",

//};
//productService.CreateProduct(addProduct);


// <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<Order CREATE>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//try
//{

//    var orderService = builder.Services.GetRequiredService<OrderService>();

//    // Skapa en ny order med några order rader
//    var newOrderDto = new NewOrderDto
//    {
//        OrderRows = new List<OrderRowDto>
//                {
//                    new OrderRowDto { ProductName = "Samsung galaxy s24", Quantity = 2 },
//                    //new OrderRowDto { ProductName = "Product2", Quantity = 1 }
//                }
//    };

//    var createdOrder = orderService.CreateOrder(newOrderDto);

//    if (createdOrder != null)
//    {
//        Console.WriteLine("Order created successfully!");

//        // info om den nya ordern
//        Console.WriteLine($"Order ID: {createdOrder.Id}");
//        Console.WriteLine($"Order Date: {createdOrder.Orderdate}");

//        foreach (var orderRow in createdOrder.OrderRows)
//        {
//            Console.WriteLine($"Product: {orderRow.Product.ProductName}, Quantity: {orderRow.Quantity}");
//        }
//    }
//    else
//    {
//        Console.WriteLine("Failed to create order.");
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"An error occurred: {ex.Message}");
//}

//<<<<<<<<<<<<<<<<<<<<<<<<<ORDERROW Get all>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
//var orderService = builder.Services.GetRequiredService<OrderService>();
//var result = orderService.GetAllOrders();
//foreach(var order in result)
//{
//    Console.WriteLine($"{order.Id} {order.Orderdate}");
//}
//var result = orderService.GetOneOrder(x => x.Id == 1);
//Console.WriteLine(result.Orderdate + " " + result.OrderRows);

//var menuService = builder.Services.GetRequiredService<MenuService>();

//menuService.StartMenu();

//var roleService = builder.Services.GetRequiredService<RoleService>();
//roleService.GetAllRoles();
//foreach (var role in roleService.GetAllRoles())
//{
//    Console.WriteLine(role.RoleName);
//}

//var categoryService = builder.Services.GetRequiredService<CategoryService>();
//var addCategory = new Category
//{
//    CategoryName = "Mobil"
//};
//categoryService.CreateCategory(addCategory);

//var getAllcategories = categoryService.GetAllCategories();
//foreach (var category in getAllcategories)
//{
//    Console.WriteLine(category.CategoryName);
//}
//var categoryId = 1;
//var getOneCategory = categoryService.GetOneCategory(x => x.Id == categoryId);
//Console.WriteLine(getOneCategory.CategoryName);

//var categoryId = 1;
//var getCategory = categoryService.GetOneCategory(x => x.Id == categoryId);
//getCategory.CategoryName = "Test";
//var result = categoryService.UpdateCategory(getCategory);

//var categoryId = 1;
//var result = categoryService.DeleteCategory(x => x.Id == categoryId);






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