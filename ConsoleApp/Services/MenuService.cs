using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Diagnostics;

namespace ConsoleApp.Services;

public class MenuService
{
    private readonly CustomerService _customerService;
    private readonly RoleService _roleService;
    private readonly AddressService _addressService;

    private readonly ProductService _productService;
    private readonly CategoryService _categoryService;
    private readonly ManufactureService _manufactureService;
    private readonly OrderService _orderService;
    private readonly OrderRowService _orderRowService;
    private readonly ReviewService _reviewService;




    public MenuService(CustomerService customerService, RoleService roleService, AddressService addressService, ProductService productService, CategoryService categoryService, ManufactureService manufactureService, OrderService orderService, OrderRowService orderRowService, ReviewService reviewService)
    {
        _customerService = customerService;
        _roleService = roleService;
        _addressService = addressService;
        _productService = productService;
        _categoryService = categoryService;
        _manufactureService = manufactureService;
        _orderService = orderService;
        _orderRowService = orderRowService;
        _reviewService = reviewService;
    }


    public void StartMenu()
    {
        while(true)
        {
            ClearAndTitle("Database Project");

            Console.WriteLine("1. Customer Menu");
            Console.WriteLine("2. Product Menu");
            Console.WriteLine("3. Order Menu");
            Console.WriteLine("4. Review Menu");

            Console.WriteLine("\n9. Exit Program");


            Console.Write("Enter your choice: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CustomerMenu();
                    break;

                case "2":
                    ProductMenu();
                    break;

                case "3":
                    OrderMenu();
                    break;

                case "4":
                    ReviewMenu();
                    break;

                case "9":
                    System.Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again");
                    PressToContinue();
                    break;
            }
        }
    }

    public void CustomerMenu()
    {
        while (true)
        {
            ClearAndTitle("Customer Menu");

            Console.WriteLine("1. Create Customer");
            Console.WriteLine("2. Get One Customer");
            Console.WriteLine("3. Get All Customer");
            Console.WriteLine("4. Update Customer");
            Console.WriteLine("5. Delete Customer");
            Console.WriteLine("6. Delete Address");
            Console.WriteLine("7. Delete Role");


            Console.WriteLine("9. Back to main menu");
            Console.Write("\n Enter your choice:");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateCustomerMenu();
                    break;
                case "2":
                    GetOneCustomerMenu();
                    break;

                case "3":
                    GetAllCustomersMenu();
                    break;

                case "4":
                    UpdateCustomerMenu();
                    break;

                case "5":
                    DeleteCustomerMenu();
                    break;

                case "6":
                    DeleteSpecificAddressMenu();
                    break;

                case "7":
                    DeleteSpecificRoleMenu();
                    break;

                case "9":
                    StartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    PressToContinue();
                    break;
            }
        }

    }

    public void CreateCustomerMenu()
    {
        try
        {
            ClearAndTitle("CREATE CUSTOMER");

            Console.Write("First name: ");
            var firstName = Console.ReadLine()!.ToLower();

            Console.Write("Lastname: ");
            var lastName = Console.ReadLine()!.ToLower();

            Console.Write("Email: ");
            var email = Console.ReadLine()!.ToLower();

            Console.Write("Rolename: ");
            var roleName = Console.ReadLine()!.ToLower();

            Console.Write("Street: ");
            var street = Console.ReadLine()!.ToLower();

            Console.Write("Postal Code: ");
            var postalCode = Console.ReadLine()!.ToLower();

            Console.Write("City: ");
            var city = Console.ReadLine()!.ToLower();

            var customerDto = _customerService.CreateCustomer(new CreateCustomerDto
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                RoleName = roleName,
                Street = street,
                PostalCode = postalCode,
                City = city
            });

            if (customerDto != null)
            {
                //Console.Clear();
                Console.WriteLine($"Customer {firstName} {lastName} has been added");
                PressToContinue();
            }
            else
            {
                //Console.Clear();
                Console.WriteLine("Something went wrong. Customer has not been added");
                PressToContinue();
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex); }
    }

    public void GetAllCustomersMenu()
    {
        try
        {
            ClearAndTitle("GET ALL CUSTOMERS");
            var customerEntity = _customerService.GetAllCustomers();

            if (customerEntity != null)
            {
                Console.WriteLine("All customers: \n");
                foreach (var customer in customerEntity)
                {
                    //var address = _addressService.GetOneAddress(x => x.Id == customer.AddressId);
                    //var roleName = _roleService.GetOneRole(x => x.Id == customer.RoleId);

                    //Console.WriteLine("_______________________________________________");
                    Console.WriteLine($"{customer.Id}. {customer.FirstName}, {customer.LastName}, {customer.Email}, {customer.Address.Street}, {customer.Address.PostalCode}, {customer.Address.City}, {customer.Role.RoleName}");
                    Console.WriteLine("______________________________________________________________________________________________");

                }
                PressToContinue();
            }
            else
            {
                Console.WriteLine("No customers found");
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetOneCustomerMenu()
    {
        try
        {
            ClearAndTitle("GET ONE CUSTOMER");

            DisplayCustomerIdAndName();

            Console.Write("Enter id to se all details: ");
            var customerId = Console.ReadLine();

            if (int.TryParse(customerId, out int inputId))
            {
                var selectedCustomer = _customerService.GetOneCustomer(x => x.Id == inputId);
                if (selectedCustomer != null)
                {
                    Console.WriteLine("\nSelected customer:");
                    Console.WriteLine($"{selectedCustomer.Id}. Firstname: {selectedCustomer.FirstName}, Lastname: {selectedCustomer.LastName}, Email: {selectedCustomer.Email}, Street: {selectedCustomer.Address.Street}, PostalCode: {selectedCustomer.Address.PostalCode}, City: {selectedCustomer.Address.City}, Role: {selectedCustomer.Role.RoleName}");


                    PressToContinue();
                }
                else
                {
                    Console.WriteLine("No customer found");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Try again!");
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public void UpdateCustomerMenu()
    {
        try
        {
            ClearAndTitle("UPDATE CUSTOMER");

            DisplayCustomerIdAndName();

            Console.Write($"Enter id to update customer: ");
            var customerId = Console.ReadLine();

            if (int.TryParse(customerId, out int inputId))
            {
                var customerEntity = _customerService.GetOneCustomer(x => x.Id == inputId);
                if (customerEntity != null)
                {
                    Console.WriteLine("\nSelected customer:");
                    Console.WriteLine($"{customerEntity.Id}. Firstname: {customerEntity.FirstName}, Lastname: {customerEntity.LastName}, Email: {customerEntity.Email}, Street: {customerEntity.Address.Street}, PostalCode: {customerEntity.Address.PostalCode}, City: {customerEntity.Address.City}, Role: {customerEntity.Role.RoleName}");



                    Console.Write("\nEnter new email: ");
                    var newEmailInput = Console.ReadLine()!.ToLower();

                    if (!string.IsNullOrWhiteSpace(newEmailInput))
                    {
                        var existingEmail = _customerService.GetOneCustomer(x => x.Email == newEmailInput);
                        if (existingEmail == null)
                        {
                            customerEntity.Email = newEmailInput;
                        }
                        else
                        {
                            Console.WriteLine("Email already exists. Try again");
                            PressToContinue();
                            return;
                        }
                    }

                    Console.Write("\nEnter new first name: ");
                    var newFirstNameInput = Console.ReadLine()!.ToLower();

                    if (!string.IsNullOrWhiteSpace(newFirstNameInput))
                    {
                        customerEntity.FirstName = newFirstNameInput;
                    }

                    Console.Write("\nEnter new lastname: ");
                    var newLastNameInput = Console.ReadLine()!.ToLower();

                    if (!string.IsNullOrWhiteSpace(newLastNameInput))
                    {
                        customerEntity.LastName = newLastNameInput;
                    }

                    Console.Write("\nEnter new streetname: ");
                    var newStreetInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newStreetInput))
                    {
                        customerEntity.Address.Street = newStreetInput;
                    }

                    Console.Write("\nEnter new postalcode: ");
                    var newPostalCodeInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newPostalCodeInput))
                    {
                        customerEntity.Address.PostalCode = newPostalCodeInput;
                    }

                    Console.Write("\nEnter new city: ");
                    var newCityInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newCityInput))
                    {
                        customerEntity.Address.City = newCityInput;
                    }

                    if (customerEntity != null)
                    {
                        Console.WriteLine("Customer was updated successfully");
                        var newCustomer = _customerService.UpdateCustomer(customerEntity);
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong, try again!");
                    }
                }

                PressToContinue();
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public bool DeleteCustomerMenu()
    {
        try
        {
            ClearAndTitle("DELETE CUSTOMER");

            DisplayCustomerIdAndName();
            Console.Write("Enter id to delete customer: ");
            var input = int.Parse(Console.ReadLine()!);
            var customerEntity = _customerService.GetOneCustomer(x => x.Id == input);



            //var result = _customerService.DeleteCustomer(x => x.Id == input);

            if (customerEntity != null)
            {
                Console.WriteLine($"Are you sure you want to delete {customerEntity.FirstName} {customerEntity.LastName}? (y/n)");
                var answer = Console.ReadLine()!.ToLower();
                if (answer.Equals("y"))
                {
                    _customerService.DeleteCustomer(x => x.Id == customerEntity.Id);
                    Console.WriteLine("Customer has been deleted successfully!");
                }
                else
                {
                    Console.WriteLine($"Customer {customerEntity.FirstName} {customerEntity.LastName} was not deleted!");
                }
            }
            else
            {
                Console.WriteLine("Something went wrong!");
            }

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    public void DeleteSpecificAddressMenu()
    {
        try
        {
            ClearAndTitle("Delete Address");

            var addresses = _addressService.GetAllAddresses();

            if(addresses != null)
            {
                foreach(var address in addresses)
                {
                    Console.WriteLine($"{address.Id}. {address.Street}, {address.PostalCode}, {address.City}");
                    Console.WriteLine("______________________________________________________");
                }

                Console.Write("Enter id to delete address: ");
                var inputId = int.Parse(Console.ReadLine()!);

                if(!_addressService.HasCustomers(inputId))
                {
                    var addressToDelete = _addressService.DeleteAddress(x => x.Id == inputId);
                    if(addressToDelete)
                    {
                        Console.WriteLine($"Address has been deleted successfully");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong, try again!");
                    }
                }
                else
                {
                    Console.WriteLine("Address ís not empty, make sure to remove all customers before deleting address");
                }
            }
            else
            {
                Console.WriteLine("Something went wrong, try again");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void DeleteSpecificRoleMenu()
    {
        try
        {
            ClearAndTitle("Delete Role");

            var roles = _roleService.GetAllRoles();

            if (roles != null)
            {
                foreach (var role in roles)
                {
                    Console.WriteLine($"{role.Id}. {role.RoleName}");
                    Console.WriteLine("______________________________________________________");
                }

                Console.Write("Enter id to delete role: ");
                var inputId = int.Parse(Console.ReadLine()!);

                if (!_roleService.HasCustomers(inputId))
                {
                    var roleToDelete = _roleService.DeleteRole(x => x.Id == inputId);
                    if (roleToDelete)
                    {
                        Console.WriteLine($"Role has been deleted successfully");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong, try again!");
                    }
                }
                else
                {
                    Console.WriteLine("Role ís not empty, make sure to remove all customers before deleting role");
                }
            }
            else
            {
                Console.WriteLine("Something went wrong, try again");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }





    private void DisplayCustomerIdAndName()
    {
        var customerEntity = _customerService.GetAllCustomers();
        if (customerEntity != null)
        {
            foreach (var customer in customerEntity)
            {
                Console.WriteLine($"{customer.Id}. {customer.FirstName} {customer.LastName}");
                Console.WriteLine("______________________________________________________");
            }
        }
        else
        {
            Console.WriteLine("No customer was found");
        }
    }

    private void ClearAndTitle(string title)
    {
        Console.Clear();
        Console.WriteLine($"{title}\n\n");
    }
    private void PressToContinue()
    {
        Console.WriteLine("\nPress any key to continue");
        Console.ReadKey();
    }












    // PRODUCT CATALOG

    public void ProductMenu()
    {
        while (true)
        {
            ClearAndTitle("Product Menu");

            Console.WriteLine("1. Create Product");
            Console.WriteLine("2. Get One Product");
            Console.WriteLine("3. Get All Product");
            Console.WriteLine("4. Update Product");
            Console.WriteLine("5. Delete Product");

            Console.WriteLine("6. Delete specific category");
            Console.WriteLine("7. Delete specific manufacture");

            Console.WriteLine("\n9. Back to stat menu");
            Console.Write("\n Enter your choice: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateProductMenu();
                    break;
                case "2":
                    GetOneProductMenu();
                    break;

                case "3":
                    GetAllProductsMenu();
                    break;

                case "4":
                    UpdateProductMenu();
                    break;

                case "5":
                    DeleteProductMenu();
                    break;

                case "6":
                    DeleteSpecificCategoryMenu();
                    break;

                case "7":
                    DeleteSpecificManufactureMenu();
                    break;

                case "9":
                    StartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid option");
                    PressToContinue();
                    break;
            }
        }

    }

    public void CreateProductMenu()
    {
        try
        {
            ClearAndTitle("CREATE PRODUCT");

            Console.Write("Enter productname: ");
            var productName = Console.ReadLine()!.ToLower();


            Console.Write("Enter product price: ");
            var productPrice = decimal.Parse(Console.ReadLine()!);


            Console.Write("Enter category: ");
            var categoryName = Console.ReadLine()!.ToLower();


            Console.Write("Enter manufacture: ");
            var manufacureName = Console.ReadLine()!.ToLower();


            Console.Write("Enter ingress: ");
            var ingress = Console.ReadLine()!.ToLower();

            Console.Write("Enter description(nullable): ");
            var descriptionText = Console.ReadLine()!.ToLower();

            var productDto = _productService.CreateProduct(new CreateProductDto {
                ProductName = productName,
                ProductPrice = productPrice,
                CategoryName = categoryName,
                ManufacureName = manufacureName,
                Ingress = ingress,
                DescriptionText = descriptionText
            });
            if (productDto != null)
            {
                //Console.Clear();
                Console.WriteLine($"Product: {productName} has been added");
                PressToContinue();
            }
            else
            {
                //Console.Clear();
                Console.WriteLine("Something went wrong. Product has not been added");
                PressToContinue();
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public void GetOneProductMenu()
    {
        ClearAndTitle("GET ONE PRODUCT");

        DisplayProductIdAndName();
        Console.Write("Enter id to see details: ");
        var productId = Console.ReadLine();

        if (int.TryParse(productId, out int inputId))
        {
            var selectedProduct = _productService.GetOneProduct(x => x.Id == inputId);
            if (selectedProduct != null)
            {
                Console.WriteLine($"Id: {selectedProduct.Id}, Productname: {selectedProduct.ProductName}, Price: {selectedProduct.ProductPrice}, Category: {selectedProduct.Category.CategoryName}, Manufacture: {selectedProduct.Manufacture.ManufactureName}, Description ingress: {selectedProduct.Description.Ingress}, Description text: {selectedProduct.Description.DescriptionText} ");

                PressToContinue();
            }
            else
            {
                Console.WriteLine("No product was found!");
            }
        }
        else
        {
            Console.Write("Invalid input");
        }
    }


    public void GetAllProductsMenu()
    {
        try
        {
            ClearAndTitle("ALL PRODUCTS");


            var products = _productService.GetAllProducts();

            foreach (var product in products)
            {
                Console.WriteLine($"Id: {product.Id}");
                Console.WriteLine($"Productname: {product.ProductName}");
                Console.WriteLine($"Price: {product.ProductPrice}");
                Console.WriteLine($"Category: {product.Category.CategoryName}");
                Console.WriteLine($"Manufacture: {product.Manufacture.ManufactureName}");
                Console.WriteLine($"Ingress: {product.Description.Ingress}");
                Console.WriteLine($"Description: {product.Description.DescriptionText}");
                Console.WriteLine("______________________________________________________________________________________________");
            }
            PressToContinue();


        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public void UpdateProductMenu()
    {
        try
        {
            ClearAndTitle("UPDATE PRODUCT");

            DisplayProductIdAndName();

            Console.Write("Enter Id to update product: ");
            var productId = Console.ReadLine();

            if (int.TryParse(productId, out int inputId))
            {
                var productEntity = _productService.GetOneProduct(x => x.Id == inputId);
                if (productEntity != null)
                {
                    Console.WriteLine("Selected product:");
                    Console.WriteLine($"Id: {productEntity.Id}, Productname: {productEntity.ProductName}, Price: {productEntity.ProductPrice}, Category: {productEntity.Category.CategoryName}, Manufacture: {productEntity.Manufacture.ManufactureName}, Ingress: {productEntity.Description.Ingress}, Description text: {productEntity.Description.DescriptionText}");

                    Console.WriteLine("\nLeave empty if you dont want to change\n");
                    Console.Write("Enter new productname: ");
                    var newProductNameInput = Console.ReadLine()!.ToLower();
                    if (!string.IsNullOrWhiteSpace(newProductNameInput))
                    {
                        productEntity.ProductName = newProductNameInput;
                    }

                    Console.Write("Enter new price: ");
                    var newProductPriceInput = Console.ReadLine();
                    if (decimal.TryParse(newProductPriceInput, out var newProductPrice))
                    {
                        productEntity.ProductPrice = newProductPrice;
                    }
                    else
                    {
                        Console.WriteLine("Wrong format! Price has not been changed");
                    }

                    Console.Write("Enter new Category: ");
                    var newCategoryNameInput = Console.ReadLine()!.ToLower();
                    if (!string.IsNullOrWhiteSpace(newCategoryNameInput))
                    {
                        // Kontrollera om kategorin redan finns
                        var existingCategory = _categoryService.GetOneCategory(x => x.CategoryName == newCategoryNameInput);

                        if (existingCategory == null)
                        {
                            // Om kategorin inte finns, skapa en ny kategori
                            var newCategory = new Category { CategoryName = newCategoryNameInput };
                            existingCategory = _categoryService.CreateCategory(newCategory);
                        }

                        // Uppdatera produktens kategori
                        productEntity.Category = existingCategory;
                    }

                    Console.Write("Enter new Manufacture: ");
                    var newManufactureNameInput = Console.ReadLine()!.ToLower();
                    if (!string.IsNullOrWhiteSpace(newManufactureNameInput))
                    {
                        productEntity.Manufacture.ManufactureName = newManufactureNameInput;
                    }

                    Console.Write("Enter new Ingress: ");
                    var newIngressInput = Console.ReadLine()!.ToLower();
                    if (!string.IsNullOrWhiteSpace(newIngressInput))
                    {
                        productEntity.Description.Ingress = newIngressInput;
                    }

                    Console.Write("Enter new Description: ");
                    var newDescriptionTextInput = Console.ReadLine()!.ToLower();
                    if (!string.IsNullOrWhiteSpace(newDescriptionTextInput))
                    {
                        productEntity.Description.DescriptionText = newDescriptionTextInput;
                    }

                    if (productEntity != null)
                    {
                        var result = _productService.UpdateProduct(productEntity);
                        if (result != null)
                        {
                            Console.WriteLine("Product has successfully been updated");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong, product has not been updated");
                        }
                    }

                }

            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public void DeleteProductMenu()
    {
        try
        {
            ClearAndTitle("DELETE PRODUCT");
            DisplayProductIdAndName();

            Console.Write("Enter id to delete product: ");
            var input = int.Parse(Console.ReadLine()!);

            var productEntity = _productService.GetOneProduct(x => x.Id == input);


            //var result = _productService.DeleteProduct(x => x.Id == input);
            if(productEntity != null)
            {
                Console.WriteLine($"Are you sure you want to delete {productEntity.ProductName}? (y/n)");
                var choice = Console.ReadLine()!.ToLower();
                if(choice.Equals("y"))
                {
                    _productService.DeleteProduct(x => x.Id == productEntity.Id);
                    Console.WriteLine($"{productEntity.ProductName} has been deleted");
                }
                else
                {
                    Console.WriteLine($"{productEntity.ProductName} was not deleted");
                }
            }
            else
            {
                Console.WriteLine("Something went wrong");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }




    public void DeleteSpecificCategoryMenu()
    {
        try
        {
            ClearAndTitle("Delete One Category");
            var categories = _categoryService.GetAllCategories();
            if (categories != null)
            {
                foreach(var category in categories)
                {
                    Console.WriteLine($"{category.Id} {category.CategoryName}");
                    Console.WriteLine("______________________________________________________");
                }

                Console.WriteLine("Enter id to delete category");
                var input = int.Parse(Console.ReadLine()!);

                if (!_categoryService.HasProducts(input))
                {
                    var deleted = _categoryService.DeleteCategory(x => x.Id == input);
                    if(deleted)
                    {
                        Console.WriteLine($"Category has been deleted successfully");
                        PressToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong, try again!");
                        PressToContinue();
                    }
                }
                else
                {
                    Console.WriteLine("Category is not empty, remove all products before deleteing category");
                    PressToContinue();
                }
            }
            else
            {
                Console.WriteLine("Failed to delete category. The category still has products associated with it.");
                PressToContinue();
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void DeleteSpecificManufactureMenu()
    {
        try
        {
            ClearAndTitle("Delete One Manufacture");
            var manufactures = _manufactureService.GetAllManufactures();
            if (manufactures != null)
            {
                foreach (var manufacture in manufactures)
                {
                    Console.WriteLine($"{manufacture.Id} {manufacture.ManufactureName}");
                    Console.WriteLine("______________________________________________________");
                }

                Console.WriteLine("Enter id to delete manufacture");
                var input = int.Parse(Console.ReadLine()!);

                if (!_manufactureService.HasProducts(input))
                {
                    var deleted = _manufactureService.DeleteManufacture(x => x.Id == input);
                    if (deleted)
                    {
                        Console.WriteLine($"Manufacture has been deleted successfully");
                        PressToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong, try again!");
                        PressToContinue();
                    }
                }
                else
                {
                    Console.WriteLine("Manufacture is not empty, remove all products before deleteing manufacture");
                    PressToContinue();
                }
            }
            else
            {
                Console.WriteLine("Failed to delete manufacture. Try again");
                PressToContinue();
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    private void DisplayProductIdAndName()
    {
        var productEntity = _productService.GetAllProducts();
        if (productEntity != null)
        {
            foreach (var product in productEntity)
            {
                Console.WriteLine($"{product.Id}. {product.ProductName}");
                Console.WriteLine("______________________________________________________");
            }
        }
        else
        {
            Console.WriteLine("No customer was found");
        }
    }


    //<<<<<<<<<<<<<<<<<<<<<<<<<ORDERs>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void OrderMenu()
    {
        try
        {
            ClearAndTitle("Order Menu");

            Console.WriteLine("1. Create Order");
            Console.WriteLine("2. Get One Order");
            Console.WriteLine("3. Get All Orders");
            Console.WriteLine("4. Update Order");
            Console.WriteLine("5. Delete Order");

            var option = Console.ReadLine();

            switch(option)
            {
                case "1":
                    CreateOrderMenu();
                    break;

                case "2":
                    GetOneOrderMenu();
                    break;

                case "3":
                    GetAllOrdersMenu();
                    break;

                case "4":
                    UpdateOrderMenu();
                    break;

                case "5":
                    DeleteOrderMenu();
                    break;

            }
        
        
                }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateOrderMenu()
    {
        try
        {
            ClearAndTitle("Create Order Menu");

            var orderRows = new HashSet<OrderRowDto>();

            while (true)
            {
                DisplayProductIdAndName();

                Console.WriteLine("Enter product to add to orderrow. When done enter order to order products");
                var input = Console.ReadLine()!.ToLower();
                if (input.Equals("order"))
                {
                    break;
                }

                var productId = int.Parse(input);
                var product = _productService.GetOneProduct(x => x.Id == productId);
                if (product != null)
                {
                    Console.Write("Enter quantity: ");
                    var quantity = int.Parse(Console.ReadLine()!);

                    var orderRowDto = new OrderRowDto
                    {
                        ProductId = productId,
                        Quantity = quantity,
                    };
                    orderRows.Add(orderRowDto);
                }
                else
                {
                    Console.WriteLine($"Product with Id {productId} was not found, try again!");
                    PressToContinue();
                }
            }

            if (orderRows.Any())
            {
                var newOrderDto = new NewOrderDto
                {
                    OrderRows = orderRows
                };

                var order = _orderService.CreateOrder(newOrderDto);
            }

            PressToContinue();


        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetAllOrdersMenu ()
    {
        try
        {
            ClearAndTitle("Show All Orders");

            var allOrderRows = _orderRowService.GetOrdersWithSameId();

            PressToContinue();
        }
        catch(Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    //eventuellt fixa
    public void GetOneOrderMenu()
    {
        try
        {
            ClearAndTitle("Show One Order");

            var allOrderRows = _orderRowService.GetOrdersWithSameId();
            //foreach(var orderRow in allOrderRows)
            //{
            //    Console.WriteLine($"{orderRow.OrderId} {orderRow.Product.ProductName}");
            //}

            Console.Write("Enter order id: ");
            var input = int.Parse(Console.ReadLine()!);

            var order = _orderRowService.GetAllOrderRows().Where(x => x.OrderId == input);

            if(order.Any())
            {
                foreach(var orderRow in order)
                {
                    Console.WriteLine($"Product: {orderRow.Product.ProductName}, Quantity: {orderRow.Quantity}, Date: {orderRow.Order.Orderdate}");

                }
            }
            else
            {
                Console.WriteLine("Orderid not found");
            }

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void UpdateOrderMenu()
    {
        try
        {
            ClearAndTitle("Update Order");

            var allOrders = _orderRowService.GetOrdersWithSameId();

            Console.Write("Enter id for the order you want to update: ");
            var orderRowId = int.Parse(Console.ReadLine()!);

            if(allOrders.Any())
            {
                foreach (var orderRow in allOrders)
                {
                    Console.WriteLine($"Id: {orderRow.Id}, {orderRow.Product.ProductName}");
                }
                Console.Write("Enter id for the order you want to update: ");
                var orderId = int.Parse(Console.ReadLine()!);
                if(allOrders.Any())
                {
                    var getOneOrderRow = _orderRowService.GetOneOrderRow(x => x.Id == orderId);
                    if(getOneOrderRow != null)
                    {
                        Console.WriteLine($"Selected orderrow: ");
                        Console.WriteLine($"Id: { getOneOrderRow.Id}, ProductName: { getOneOrderRow.Product.ProductName} Quantity: {getOneOrderRow.Quantity}");

                        Console.WriteLine("\nLeave empty if you dont want to change\n");
                        Console.Write("Enter new quantity: ");
                        var newQuantityInput = int.Parse(Console.ReadLine()!);
                        if (newQuantityInput > 0)
                        {
                            getOneOrderRow.Quantity = newQuantityInput;
                        }


                        Console.Write("Update product ");
                        Console.WriteLine("\nProduct list: ");
                        var products = _productService.GetAllProducts();
                        foreach(var product in products)
                        {
                            Console.WriteLine($"Id: {product.Id}, productname: {product.ProductName}");
                        }
                        Console.Write($"\nEnter id for product you want to chage to: ");
                        var newProductId = int.Parse(Console.ReadLine()!);
                        var newProduct = _productService.GetOneProduct(x => x.Id == newProductId);

                        if(newProduct != null)
                        {
                            getOneOrderRow.Product = newProduct;
                            _orderRowService.UpdateOrderRow(getOneOrderRow);
                            Console.WriteLine($"Changed product to {getOneOrderRow.Product.ProductName} successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Something went wrong, try again!");
                        }

                    }
                }
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public void DeleteOrderMenu()
    {
        try
        {
            ClearAndTitle("Delete Order");

            var orderRows = _orderRowService.GetOrdersWithSameId();
            Console.WriteLine("Which order do you want to delete?");
            var inputId = int.Parse(Console.ReadLine()!);

            var result = _orderRowService.DeleteOrderRow(x => x.OrderId == inputId);
            if (result)
            {
                Console.WriteLine($"Order with OrderId {inputId} has been deleted.");
            }
            else
            {
                Console.WriteLine($"No order found with OrderId {inputId}.");
            }

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }






    //<<<<<<<<<<<<<<<<<<<<Review Menu>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

    public void ReviewMenu()
    {
        try
        {
            ClearAndTitle("Review Menu");
            Console.WriteLine("1. Create Review");
            Console.WriteLine("2. Get Reviews For Specific Product");
            Console.WriteLine("3. Get All Reviews");
            Console.WriteLine("4. Update Review");
            Console.WriteLine("5. Delete Review");
            Console.Write("Enter option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateReview();
                    break;

                case "2":
                    GetReviewsForOneProduct();
                    break;

                case "3":
                    GetAllReviews();
                    break;

                case "4":
                    UpdateReview();
                    break;

                case "5":
                    DeleteReview();
                    break;
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public void CreateReview()
    {
        try
        {
            ClearAndTitle("Review Menu");

            DisplayProductIdAndName();

            Console.Write("Enter id for product to review: ");
            var productId = int.Parse(Console.ReadLine()!);

            var product = _productService.GetOneProduct(x => x.Id == productId);
            if(product != null)
            {
                Console.WriteLine($"{product.Id}. {product.ProductName}");
                Console.WriteLine("\nEnter review text:");
                var reviewText = Console.ReadLine();
                if(reviewText != null)
                {
                    var reviewEntity = new Review
                    {
                        ReviewText = reviewText,
                        ReviewDate = DateTime.Now,
                        ProductId = productId
                    };

                    _reviewService.CreateReview(reviewEntity);
                }
                else
                {
                    Console.WriteLine("Something went wrong! Could not create review");
                }
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetReviewsForOneProduct()
    {
        try
        {
            ClearAndTitle("Get Reviews For Specific Product");

            DisplayProductIdAndName();


            Console.Write("Enter id to see reviews: ");
            var productId = int.Parse(Console.ReadLine()!);

            var reviews = _reviewService.GetAllReviews().Where(x => x.ProductId == productId);

            if(reviews.Any())
            {
                var reviewCount = reviews.Count();
                Console.WriteLine($"This product has {reviewCount} {(reviewCount > 1 ? "reviews" : "review")}");
                foreach(var review in reviews)
                {
                    Console.WriteLine($"For product: {review.Product.ProductName}");
                    Console.WriteLine($"Review: {review.ReviewText}");
                    Console.WriteLine($"Created: {review.ReviewDate}\n");

                }
            }
            else
            {
                Console.WriteLine("No Reviews found");
            }

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetAllReviews()
    {
        try
        {
            ClearAndTitle("Get All Reviews");

            var allReviews = _reviewService.GetReviewsWithSameId();

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void UpdateReview()
    {
        try
        {
            ClearAndTitle("Update Review");

            var allReviews = _reviewService.GetReviewsWithSameId();
            Console.Write("Enter productid to update review text:");
            var productId = int.Parse(Console.ReadLine()!);

            var reviewsForProduct = allReviews.Where(x => x.Product.Id == productId).ToList();
            if (reviewsForProduct.Any())
            {
                Console.WriteLine($"\nReviews for product id {productId}:");
                foreach (var review in reviewsForProduct)
                {
                    Console.WriteLine($"Review id: {review.Id}. ReviewText: {review.ReviewText}, Date of review: {review.ReviewDate}        (Product: {review.Product.ProductName})");
                }

                Console.Write("Enter id for the review you want to update: ");
                var reviewId = int.Parse(Console.ReadLine()!);
                var reviewToUpdate = reviewsForProduct.FirstOrDefault(x => x.Id == reviewId);
                if (reviewToUpdate != null)
                {
                    Console.WriteLine($"Selected review: ");
                    Console.WriteLine($"Review id: {reviewToUpdate.Id}. ReviewText: {reviewToUpdate.ReviewText}, Date of review: {reviewToUpdate.ReviewDate}        (Product: {reviewToUpdate.Product.ProductName})");

                    Console.WriteLine("\nEnter new review text: ");
                    var newReviewText = Console.ReadLine();
                    if (!string.IsNullOrEmpty(newReviewText))
                    {
                        reviewToUpdate.ReviewText = newReviewText;
                        _reviewService.UpdateReview(reviewToUpdate);
                        Console.WriteLine($"Updated review text successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Review text cannot be empty. No changes were made.");
                    }
                }
                else
                {
                    Console.WriteLine($"Review with id {reviewId} not found for product id {productId}.");
                }
            }
            else
            {
                Console.WriteLine($"No reviews found for product id {productId}.");
            }

            PressToContinue();

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }


    public void DeleteReview()
    {
        try
        {
            ClearAndTitle("Delete Review");

            var allReviews = _reviewService.GetReviewsWithSameId();

            Console.Write("Enter productid to remove review: ");
            var productId = int.Parse(Console.ReadLine()!);

            var reviewsForProduct = allReviews.Where(x => x.ProductId == productId).ToList();
            if (reviewsForProduct.Any())
            {
                Console.WriteLine($"\nReviews for product id {productId}:");
                foreach (var review in reviewsForProduct)
                {
                    Console.WriteLine($"Review id: {review.Id}. ReviewText: {review.ReviewText}, Date of review: {review.ReviewDate}        (Product: {review.Product.ProductName})");
                }

                Console.Write("\nEnter id for the review you want to delete: ");
                var reviewId = int.Parse(Console.ReadLine()!);

                var reviewToDelete = reviewsForProduct.FirstOrDefault(x => x.Id == reviewId);
                if(reviewToDelete != null)
                {
                    var result = _reviewService.DeleteReview(x => x.Id == reviewId);
                    if(result)
                    {
                        Console.WriteLine($"Review {reviewToDelete.Id} has been deleted successfully");
                    }
                }
                else
                {
                    Console.WriteLine("Something went wrong, try again!");
                }

            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
}
