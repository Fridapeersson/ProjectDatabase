using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Repositories;
using DbProject.Services;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Data;
using System.Diagnostics;
using System.Net;

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
    private readonly DescriptionService _descriptionService;





    public MenuService(CustomerService customerService, RoleService roleService, AddressService addressService, ProductService productService, CategoryService categoryService, ManufactureService manufactureService, OrderService orderService, OrderRowService orderRowService, ReviewService reviewService, DescriptionService descriptionService)
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
        _descriptionService = descriptionService;
    }


    public void StartMenu()
    {
        while(true)
        {
            ClearAndTitle("Database Project");

            Console.WriteLine("1. Customer Menu");
            Console.WriteLine("2. Address Menu");
            Console.WriteLine("3. Role Menu");
            Console.WriteLine("4. Product Menu");
            Console.WriteLine("5. Category Menu");
            Console.WriteLine("6. Manufacture Menu");
            Console.WriteLine("7. Order Menu");
            Console.WriteLine("8. Review Menu");
            Console.WriteLine("9. description Menu");


            Console.WriteLine("\n10. Exit Program");


            Console.Write("Enter your choice: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CustomerMenu();
                    break;

                case "2":
                    AddressMenu();
                    break;

                case "3":
                    RoleMenu();
                    break;

                case "4":
                    ProductMenu();
                    break;

                case "5":
                    CategoryMenu();
                    break;

                case "6":
                    ManufactureMenu();
                    break;

                case "7":
                    OrderMenu();
                    break;

                case "8":
                    ReviewMenu();
                    break;

                case "9":
                    DescriptionMenu();
                    break;

                case "10":
                    System.Environment.Exit(0);
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again");
                    PressToContinue();
                    break;
            }
        }
    }


    //<<<<<<<<<<<<<<<<<CUSTOMERS>>>>>>>>>>>>>>>>>>>>>
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

            var existingAddress = _addressService.GetOneAddress(x => x.Street.ToLower() == street && x.PostalCode.ToLower() == postalCode && x.City.ToLower() == city);

            if (existingAddress != null)
            {
                // Skapa kund med befintlig address
                var customerDto = _customerService.CreateCustomer(new CreateCustomerDto
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    RoleName = roleName,
                    AddressId = existingAddress.Id,
                    Address = existingAddress
                });

                if (customerDto != null)
                {
                    Console.WriteLine($"Customer {firstName} {lastName} has been added (with existing address)");
                    PressToContinue();
                }
                else
                {
                    Console.WriteLine("Something went wrong. Customer has not been added");
                    PressToContinue();
                }
            }
            else
            {
                // Skapa ny address och kund
                var newAddressDto = _addressService.CreateAddress(new AddressEntity
                {
                    Street = street,
                    PostalCode = postalCode,
                    City = city
                });

                if (newAddressDto != null)
                {
                    var customerDto = _customerService.CreateCustomer(new CreateCustomerDto
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        RoleName = roleName,
                        AddressId = newAddressDto.Id,
                        Address = newAddressDto
                    });

                    if (customerDto != null)
                    {
                        Console.WriteLine($"Customer {firstName} {lastName} has been added with new address");
                        PressToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong. Customer has not been added");
                        PressToContinue();
                    }
                }
                else
                {
                    Console.WriteLine("Something went wrong. Address has not been added");
                    PressToContinue();
                }
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
            ClearAndTitle("Update Customer");

            DisplayCustomerIdAndName();

            Console.Write($"Enter id to update customer: ");
            var customerId = Console.ReadLine();

            if (int.TryParse(customerId, out int inputId))
            {
                var customerEntity = _customerService.GetOneCustomer(x => x.Id == inputId);
                if (customerEntity != null)
                {
                    Console.WriteLine("\nSelected customer:");
                    Console.WriteLine($"{customerEntity.Id}. Firstname: {customerEntity.FirstName}, Lastname: {customerEntity.LastName}, Email: {customerEntity.Email}");

                    Console.Write("\nEnter new email: ");
                    var newEmailInput = Console.ReadLine()?.ToLower();

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
                    var newFirstNameInput = Console.ReadLine()?.ToLower();

                    if (!string.IsNullOrWhiteSpace(newFirstNameInput))
                    {
                        customerEntity.FirstName = newFirstNameInput;
                    }

                    Console.Write("\nEnter new lastname: ");
                    var newLastNameInput = Console.ReadLine()?.ToLower();

                    if (!string.IsNullOrWhiteSpace(newLastNameInput))
                    {
                        customerEntity.LastName = newLastNameInput;
                    }

                    Console.Write("\nEnter new streetname: ");
                    var newStreetInput = Console.ReadLine();

                    Console.Write("\nEnter new postalcode: ");
                    var newPostalCodeInput = Console.ReadLine();

                    Console.Write("\nEnter new city: ");
                    var newCityInput = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newStreetInput) || !string.IsNullOrWhiteSpace(newPostalCodeInput) || !string.IsNullOrWhiteSpace(newCityInput))
                    {
                        var existingAddress = _addressService.GetOneAddress(x => x.Street == newStreetInput && x.PostalCode == newPostalCodeInput && x.City == newCityInput);

                        if (existingAddress != null)
                        {
                            customerEntity.Address = existingAddress;
                        }
                        else
                        {
                            var newAddress = new AddressEntity
                            {
                                Street = newStreetInput!,
                                PostalCode = newPostalCodeInput!,
                                City = newCityInput!
                            };

                            // kollar om den nya adressen redan finns i databasen
                            var existingAddressEntity = _addressService.GetOneAddress(x => x.Street == newAddress.Street && x.PostalCode == newAddress.PostalCode && x.City == newAddress.City);

                            if (existingAddressEntity != null)
                            {
                                customerEntity.Address = existingAddressEntity;
                            }
                            else
                            {
                                // gör en ny adress om den inte redan finns i databasen
                                var createdAddressEntity = _addressService.CreateAddress(newAddress);
                                if (createdAddressEntity != null)
                                {
                                    customerEntity.Address = createdAddressEntity;
                                }
                                else
                                {
                                    Console.WriteLine("Error creating new address. Customer was not updated.");
                                    PressToContinue();
                                    return;
                                }
                            }
                        }
                    }

                    // uppdaterar kunden
                    var updatedCustomer = _customerService.UpdateCustomer(customerEntity);
                    if (updatedCustomer != null)
                    {
                        Console.WriteLine("Customer was updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Error updating customer.");
                    }
                }
                else
                {
                    Console.WriteLine("Customer not found.");
                }
                PressToContinue();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
        }
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



    //<<<<<<<<<<<<<<<<<<<<<<<<<<Address>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void AddressMenu()
    {
        try
        {
            ClearAndTitle("Address Menu");


            Console.WriteLine("1. Create Address");
            Console.WriteLine("2. Get One Address");
            Console.WriteLine("3. Get All Addresses");
            Console.WriteLine("4. Update Address");
            Console.WriteLine("5. Delete Address");

            Console.WriteLine("9. Back to main menu");
            Console.Write("Enter your option: ");
            var option = Console.ReadLine();

            switch(option)
            {
                case "1":
                    CreateAddressMenu();
                    break;

                case "2":
                    GetOneAddressMenu();
                    break;

                case "3":
                    GetAllAddressesMenu();
                    break;

                case "4":
                    UpdateAddressMenu();
                    break;

                case "5":
                    DeleteSpecificAddressMenu();
                    break;

                case "9":
                    StartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again");
                    PressToContinue();
                    break;
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
    
    public void CreateAddressMenu()
    {
        try
        {
            ClearAndTitle("CreateAddress");

            Console.Write("Enter Street name: ");
            var streetName = Console.ReadLine();

            Console.Write("Enter postalcode: ");
            var postalCode = Console.ReadLine();

            Console.Write("Enter City: ");
            var city = Console.ReadLine();

            var existingAddress = _addressService.GetOneAddress(x => x.Street == streetName &&  x.PostalCode == postalCode && x.City == city);
            if(existingAddress == null)
            {
                var addressEntity = new AddressEntity
                {
                    Street = streetName!,
                    PostalCode = postalCode!,
                    City = city!
                };
                if (addressEntity != null)
                {
                    _addressService.CreateAddress(addressEntity);
                    Console.WriteLine("Address was created successfully");
                }
                else
                {
                    Console.WriteLine("Failed to create address");
                }
            }
            else
            {
                Console.WriteLine("Address already exists");
            }

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetOneAddressMenu()
    {
        try
        {
            ClearAndTitle("Get One Address");

            var addresses = _addressService.GetAllAddresses();
            foreach(var address in addresses)
            {
                Console.WriteLine($"Id: {address.Id}. Street name: {address.Street}");
                Console.WriteLine("______________________________________________________");
            }
            Console.Write("Enter id to view details: ");
            var addressId = int.Parse(Console.ReadLine()!);

            var addressEntity = _addressService.GetOneAddress(x => x.Id ==  addressId);

            if(addressEntity != null)
            {
                Console.WriteLine($"Id: {addressEntity.Id}. Street name: {addressEntity.Street} Postal code: {addressEntity.PostalCode} City: {addressEntity.City}");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetAllAddressesMenu()
    {
        try
        {
            ClearAndTitle("Get All Addresses");

            var addresses = _addressService.GetAllAddresses();
            foreach( var address in addresses)
            {
                Console.WriteLine($"Id: {address.Id}. Street name: {address.Street} Postal code: {address.PostalCode} City: {address.City}");
                Console.WriteLine("___________________________________________________________________");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void UpdateAddressMenu()
    {
        try
        {
            ClearAndTitle("Update Address");

            var addressEntities = _addressService.GetAllAddresses();
            foreach (var address in addressEntities)
            {
                Console.WriteLine($"Id: {address.Id}. Street name: {address.Street}, Postalcode: {address.PostalCode}, City: {address.City})");
            }
            Console.Write("\nEnter id for the address you want to update: ");
            var addressId = int.Parse(Console.ReadLine()!);

            var addressEntity = _addressService.GetOneAddress(x => x.Id == addressId);
            if (addressEntity != null)
            {
                // Hämta kunderna kopplade till den valda adressen
                var customersOnAddress = _customerService.GetAllCustomers().Where(x => x.Address.Id == addressId);


                Console.WriteLine("Customers on specific address: ");
                foreach (var customer in customersOnAddress)
                {
                    Console.WriteLine($"Id: {customer.Id}. Firstname: {customer.FirstName}, Lastname: {customer.LastName}, Email: {customer.Email}");
                }

                Console.Write("Enter customer´s id you want to update the address for: ");
                var customerId = int.Parse(Console.ReadLine()!);

                var customerEntity = _customerService.GetOneCustomer(x => x.Id == customerId && x.Address.Id == addressId);

                if (customerEntity != null)
                {
                    Console.WriteLine("\nSelected customer:");
                    Console.WriteLine($"{customerEntity.Id}. Firstname: {customerEntity.FirstName}, Lastname: {customerEntity.LastName}, Email: {customerEntity.Email}");

                    Console.WriteLine("\nSelected address:");
                    Console.WriteLine($"Id: {addressEntity.Id}. Street name: {addressEntity.Street}, Postalcode: {addressEntity.PostalCode}, City: {addressEntity.City})");

                    Console.Write("\nEnter new street name: ");
                    var newStreetInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(newStreetInput))
                    {
                        Console.WriteLine("You need to enter a streetname");
                        PressToContinue();
                        return;
                    }

                    Console.Write("\nEnter new postalcode: ");
                    var newPostalCodeInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(newPostalCodeInput))
                    {
                        Console.WriteLine("You need to enter a postalcode");
                        PressToContinue();
                        return;
                    }

                    Console.Write("\nEnter new city: ");
                    var newCityInput = Console.ReadLine();
                    if (string.IsNullOrEmpty(newCityInput))
                    {
                        Console.WriteLine("You need to enter a city");
                        PressToContinue();
                        return;
                    }

                    // Kontrollera om den nya adressen redan finns i databasen
                    var existingAddress = _addressService.GetOneAddress(x => x.Street == newStreetInput && x.PostalCode == newPostalCodeInput && x.City == newCityInput);

                    if (existingAddress == null)
                    {
                        // Skapa ny address om den inte finns i db
                        existingAddress = new AddressEntity
                        {
                            Street = newStreetInput!,
                            PostalCode = newPostalCodeInput!,
                            City = newCityInput!,
                        };
                    }

                    // Uppdatera den valda kundens adress
                    customerEntity.Address = existingAddress;
                    _customerService.UpdateCustomer(customerEntity);

                    Console.WriteLine("Address updated successfully");
                }
                else
                {
                    Console.WriteLine("Customer not found or not associated with the selected address.");
                }
            }
            else
            {
                Console.WriteLine("Address not found.");
            }

            PressToContinue();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
        }
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

    //<<<<<<<<<<<<<<<<<<<<<<<ROLE>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void RoleMenu()
    {
        try
        {
            ClearAndTitle("Role Menu");


            Console.WriteLine("1. Create Role");
            Console.WriteLine("2. Get One Role");
            Console.WriteLine("3. Get All Roles");
            Console.WriteLine("4. Update Role");
            Console.WriteLine("5. Delete Role");

            Console.WriteLine("9. Back to main menu");

            Console.Write("Enter your option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateRoleMenu();
                    break;

                case "2":
                    GetOneRoleMenu();
                    break;

                case "3":
                    GetAllRolesMenu();
                    break;

                case "4":
                    UpdateRoleMenu();
                    break;

                case "5":
                    DeleteSpecificRoleMenu();
                    break;

                case "9":
                    StartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again");
                    PressToContinue();
                    break;
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateRoleMenu()
    {
        try
        {
            ClearAndTitle("Create Role");

            Console.Write("Enter Role name: ");
            var roleName = Console.ReadLine()!.ToLower();

            var existingRole = _roleService.GetOneRole(x => x.RoleName == roleName);

            if(existingRole == null)
            {
                var roleEntity = _roleService.CreateRole(new RoleEntity
                {
                    RoleName = roleName!,
                });

                _roleService.CreateRole(roleEntity);
                Console.WriteLine("A new role has been added!");
            }
            else
            {
                Console.WriteLine("Role already exists!");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetOneRoleMenu()
    {
        try
        {
            ClearAndTitle("Get One Role");

            var roles = _roleService.GetAllRoles();
            foreach(var role in roles )
            {
                Console.WriteLine($"{role.Id}. Role name: {role.RoleName}");
                Console.WriteLine("______________________________________________________");
            }
            Console.Write("Enter id: ");
            var roleId = int.Parse(Console.ReadLine()!);

            var roleEntity = _roleService.GetOneRole(x => x.Id == roleId);
            if(roleEntity != null)
            {
                Console.WriteLine($"RoleId: {roleEntity.Id}. Role name: {roleEntity.RoleName} ");

                if(roleEntity.Customers.Any())
                {
                    Console.WriteLine("\nCustomers connected to role:");
                    foreach(var customer in roleEntity.Customers)
                    {
                        Console.WriteLine($"customer id: {customer.Id}, Customer name: {customer.FirstName} {customer.LastName} Email: {customer.Email}");
                        Console.WriteLine("______________________________________________________");
                    }
                }
                else
                {
                    Console.WriteLine("No customers is currently in this role");
                }
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetAllRolesMenu()
    {
        try
        {
            ClearAndTitle("Get All Roles");

            var roles = _roleService.GetAllRoles();
            foreach( var role in roles)
            {
                Console.WriteLine($"RoleId: {role.Id}, RoleName: {role.RoleName}");
                Console.WriteLine("______________________________________________________");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void UpdateRoleMenu()
    {
        try
        {
            ClearAndTitle("Update Role");

            var roles = _roleService.GetAllRoles();
            foreach(var role in roles)
            {
                Console.WriteLine($"RoleId: {role.Id}, RoleName: {role.RoleName}");
                Console.WriteLine("______________________________________________________");
            }
            Console.Write("Enter id for the role you want to update: ");
            var roleId = int.Parse(Console.ReadLine()!);

            var roleEntity = _roleService.GetOneRole(x => x.Id == roleId);
            if (roleEntity != null)
            {
                Console.WriteLine($"RoleId: {roleEntity.Id}. Role name: {roleEntity.RoleName} ");

                if (roleEntity.Customers.Any())
                {
                    Console.WriteLine("\nCustomers connected to role:");
                    foreach (var customer in roleEntity.Customers)
                    {
                        Console.WriteLine($"customer id: {customer.Id}, Customer name: {customer.FirstName} {customer.LastName} Email: {customer.Email}");
                        Console.WriteLine("______________________________________________________");
                    }
                    Console.Write("Enter customer´s id you want to update role for: ");
                    var customerId = int.Parse(Console.ReadLine()!);

                    var customerEntity = _customerService.GetOneCustomer(x => x.Id == customerId);
                    if (customerEntity != null)
                    {
                        Console.WriteLine("\nSelected customer:");
                        Console.WriteLine($"Customer id: {customerEntity.Id}, customer name: {customerEntity.FirstName} {customerEntity.LastName}, Email: {customerEntity.Email}");

                        Console.Write("\nEnter new Role name: ");
                        var roleName = Console.ReadLine()!;
                        if(string.IsNullOrEmpty(roleName) || string.IsNullOrWhiteSpace(roleName))
                        {
                            Console.WriteLine("You have to enter a role name");
                            PressToContinue();
                            return;
                        }

                        //kolla om uppdatederade rollen finns i databasen
                        var existingRole = _roleService.GetOneRole(x => x.RoleName == roleName);
                        if(existingRole == null)
                        {
                            //skapa ny roll
                            existingRole = new RoleEntity
                            {
                                RoleName = roleName!
                            };
                        }
                        //uppdatera kundens rollnamn
                        customerEntity.Role = existingRole;
                        _customerService.UpdateCustomer(customerEntity);
                        Console.WriteLine("Updated successfully!");
                    }
                }
                else
                {
                    Console.WriteLine("No customers is currently in this role");
                }
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

            Console.WriteLine("\n9. Back to main menu");
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
            ClearAndTitle("Create Product");

            Console.Write("Enter productname: ");
            var productName = Console.ReadLine()!.ToLower();
            // Kontrollera om produkten redan finns i databasen
            var existingProduct = _productService.GetOneProduct(x => x.ProductName == productName);
            if (existingProduct != null)
            {
                Console.WriteLine("Product already exists.");
                PressToContinue();
                return;
            }


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

        ClearAndTitle("Get One Product");

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
            ClearAndTitle("Update Category");

            var categories = _categoryService.GetAllCategories();
            foreach (var category in categories)
            {
                Console.WriteLine($"Category id: {category.Id} Category name: {category.CategoryName}");
                Console.WriteLine("______________________________________________________");
            }

            Console.Write("Enter id for the category you want to update: ");
            var categoryId = int.Parse( Console.ReadLine()!);

            var categoryEntity = _categoryService.GetOneCategory(x => x.Id == categoryId);
            if(categoryEntity != null)
            {
                Console.WriteLine($"Selected Category: \nCategory id: {categoryEntity.Id} Category name: {categoryEntity.CategoryName}");

                if(categoryEntity.Products.Any())
                {
                    Console.WriteLine("\nProducts connected to category: ");
                    foreach(var product in categoryEntity.Products)
                    {
                        Console.WriteLine($"Product id: {product.Id}, product name: {product.ProductName} price: {product.ProductPrice}");
                        Console.WriteLine("______________________________________________________");
                    }
                    Console.Write("Enter product´s id to update category for specific product: ");
                    var productId = int.Parse( Console.ReadLine()!);

                    var productEntity = _productService.GetOneProduct(x => x.Id == productId);
                    if(productEntity != null)
                    {
                        Console.WriteLine("Selected product: ");
                        Console.WriteLine($"Product id: {productEntity.Id}. Product name: {productEntity.ProductName}, price: {productEntity.ProductPrice}, ((category: {productEntity.Category.CategoryName}))");

                        Console.Write("Enter new Category name: ");
                        var newCategoryName = Console.ReadLine()!;

                        if(string.IsNullOrWhiteSpace(newCategoryName))
                        {
                            Console.WriteLine("You have to enter a category name");
                            PressToContinue();
                            return;
                        }

                        var existingCategory = _categoryService.GetOneCategory(x => x.CategoryName == newCategoryName);
                        if (existingCategory == null)
                        {
                            existingCategory = new Category { CategoryName = newCategoryName };
                        }
                        productEntity.Category = existingCategory;
                        _productService.UpdateProduct(productEntity);
                        Console.WriteLine("Updated successfully");
                    }
                }
                else
                {
                    Console.WriteLine("Failed to update");
                }
                PressToContinue();
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



    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<Category>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void CategoryMenu()
    {
        try
        {
            ClearAndTitle("Category Menu");

            Console.WriteLine("1. Create Category");
            Console.WriteLine("2. Get One Category");
            Console.WriteLine("3. Get All Categories");
            Console.WriteLine("4. Update Category");
            Console.WriteLine("5. Delete Category");

            Console.WriteLine("\n9. Back to stat menu");
            Console.Write("\n Enter your choice: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateCategoryMenu();
                    break;
                case "2":
                    GetOneCategoryMenu();
                    break;

                case "3":
                    GetAllCategoriesMenu();
                    break;

                case "4":
                    UpdateCategoryMenu();
                    break;

                case "5":
                    DeleteCategoryMenu();
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
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateCategoryMenu()
    {
        try
        {
            ClearAndTitle("Create Category");

            Console.Write("Enter category name: ");
            var categoryName = Console.ReadLine()!.ToLower();
            // Kontrollera om kategorin redan finns i databasen
            var existingCategory = _categoryService.GetOneCategory(x => x.CategoryName == categoryName);
            if (existingCategory != null)
            {
                Console.WriteLine("Category already exists.");
                PressToContinue();
                return;
            }

            var newCategoryName = _categoryService.CreateCategory(new Category { CategoryName = categoryName });
            if(newCategoryName != null)
            {
                Console.WriteLine($"Category: {newCategoryName.CategoryName} has been added");
                PressToContinue();
            }
            else
            {
                Console.WriteLine("Something went wrond, category has not been added");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
    public void GetOneCategoryMenu()
    {
        try
        {
            ClearAndTitle("Get One Category");

            var categories = _categoryService.GetAllCategories();
            foreach(var category in categories)
            {
                Console.WriteLine($"Category id: {category.Id}. Category name: {category.CategoryName}");
            }

            Console.Write("Enter id to see details: ");
            var categoryId = Console.ReadLine();

            if (int.TryParse(categoryId, out int inputId))
            {
                var selectedCategory = _categoryService.GetOneCategory(x => x.Id == inputId);
                if (selectedCategory != null)
                {
                    Console.WriteLine($"Id: {selectedCategory.Id}, Category name: {selectedCategory.CategoryName}");
                    Console.WriteLine($"Products conneted to the category: ");
                    foreach(var product in selectedCategory.Products)
                    {
                        Console.WriteLine($"Product id: {product.Id}, ProductName: {product.ProductName}, Price: {product.ProductPrice}");
                    }

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
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
    public void GetAllCategoriesMenu()
    {
        try
        {
            ClearAndTitle("Get All Categories");

            var categories = _categoryService.GetAllCategories();
            foreach(var category in categories)
            {
                Console.WriteLine($"Category id: {category.Id}. Category name: {category.CategoryName}");
                Console.WriteLine("_____________________________________________________");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
    public void UpdateCategoryMenu()
    {
        try
        {
            ClearAndTitle("Update Category");

            var categories = _categoryService.GetAllCategories();
            foreach (var category in categories)
            {
                Console.WriteLine($"Category id: {category.Id}, Category name: {category.CategoryName}");
                Console.WriteLine("______________________________________________________");
            }

            Console.WriteLine("Enter id for the category you want to update: ");
            var categoryId = int.Parse(Console.ReadLine()!);

            var categoryEntity = _categoryService.GetOneCategory(x => x.Id == categoryId);
            if(categoryEntity != null)
            {
                Console.WriteLine($"Category id: {categoryEntity.Id}, Category name: {categoryEntity.CategoryName}");

                if(!categoryEntity.Products.Any())
                {
                    Console.WriteLine("No products is currently in the category");
                }
                else
                {
                    Console.WriteLine($"\n Products associated with category:");
                    foreach(var product in categoryEntity.Products)
                    {
                        Console.WriteLine($"Product id: {product.Id}. Product name: {product.ProductName}, price: {product.ProductPrice}");
                        Console.WriteLine("______________________________________________________");
                    }

                    Console.WriteLine("Enter product´s id you want to update category for: ");
                    var productId = int.Parse(Console.ReadLine()!);

                    var productEntity = _productService.GetOneProduct(x => x.Id == productId);
                    if(productEntity != null)
                    {
                        Console.WriteLine("Selected product:");
                        Console.WriteLine($"Product id: {productEntity.Id}. Product name: {productEntity.ProductName}, price: {productEntity.ProductPrice}");

                        Console.Write("\nEnter category name: ");
                        var categoryName = Console.ReadLine();
                        if(string.IsNullOrWhiteSpace(categoryName))
                        {
                            Console.WriteLine("You have to enter a category name");
                            PressToContinue();
                            return;
                        }
                        var existingCategory = _categoryService.GetOneCategory(x => x.CategoryName == categoryName);
                        if (existingCategory == null)
                        {
                            existingCategory = new Category { CategoryName = categoryName };
                        }

                        productEntity.Category = existingCategory;
                        _productService.UpdateProduct(productEntity);
                        Console.WriteLine("Updated successfully!");
                    }
                }
            }
            PressToContinue();
        }
        catch(Exception ex) { Console.WriteLine("ERROR :: "+ ex.Message); }
    }
    public void DeleteCategoryMenu()
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



    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<Manufacture>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void ManufactureMenu()
    {
        try
        {
            ClearAndTitle("Manufacturer Menu");

            Console.WriteLine("1. Create Manufacture");
            Console.WriteLine("2. Get One Manufacture");
            Console.WriteLine("3. Get All Manufacturer");
            Console.WriteLine("4. Update Manufacture");
            Console.WriteLine("5. Delete Manufacture");


            Console.WriteLine("\n9. Back to main menu");
            Console.Write("\n Enter your choice: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    CreateManufactureMenu();
                    break;
                case "2":
                    GetOneManufactureMenu();
                    break;

                case "3":
                    GetAllManufacturersMenu();
                    break;

                case "4":
                    UpdateManufactureMenu();
                    break;

                case "5":
                    DeleteManufactureMenu();
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
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateManufactureMenu()
    {
        try
        {
            ClearAndTitle("Create Manufacturer");

            Console.Write("Enter Manufacture name: ");
            var manufactureName = Console.ReadLine()!.ToLower();

            //kolla om tillverkaren redan finns i databasen
            var existingManufacture = _manufactureService.GetOneManufacture(x => x.ManufactureName == manufactureName);
            if(existingManufacture != null)
            {
                Console.WriteLine($"Manufacturer already exists.");
                PressToContinue();
                return;
            }

            var newManufactureName = _manufactureService.CreateManufacture(new Manufacture { ManufactureName = manufactureName });
            if(newManufactureName != null)
            {
                Console.WriteLine($"Manufacturer {newManufactureName.ManufactureName} has been added!");
            }
            else
            {
                Console.WriteLine("Something went wrong, manufacturer has not been added");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetOneManufactureMenu()
    {
        try
        {
            ClearAndTitle("Get One Manufacture");

            var manufacturers = _manufactureService.GetAllManufactures();
            foreach( var manufacture in manufacturers )
            {
                Console.WriteLine($"Manufacture id: {manufacture.Id}. Manufacture name: {manufacture.ManufactureName}");
            }
            Console.Write("Enter id to se details: ");
            var manufactureId = int.Parse(Console.ReadLine()!);
            var selectedManufacture = _manufactureService.GetOneManufacture(x => x.Id == manufactureId);
            if(selectedManufacture != null)
            {
                Console.WriteLine($"\nManufacture id: {selectedManufacture.Id}. Manufacture name: {selectedManufacture.ManufactureName}");
                Console.WriteLine("Products connected to the manufacturer:");
                foreach(var product in selectedManufacture.Products)
                {
                    Console.WriteLine($"Product id: {product.Id}. Product name: {product.ProductName}, price: {product.ProductPrice}");
                }
                PressToContinue();
            }
            else
            {
                Console.WriteLine("No product was found");
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetAllManufacturersMenu()
    {
        try
        {
            ClearAndTitle("Get All Manufacturers");

            var manufacturers = _manufactureService.GetAllManufactures();
            foreach(var manufacture in manufacturers )
            {
                Console.WriteLine($"Manufacture id: {manufacture.Id}. Manufacture name: {manufacture.ManufactureName}");
                Console.WriteLine("_____________________________________________________");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void UpdateManufactureMenu()
    {
        try
        {
            ClearAndTitle("Update Manufacture");

            var manufacturers = _manufactureService.GetAllManufactures();
            foreach (var manufacture in manufacturers)
            {
                Console.WriteLine($"Manufacture id: {manufacture.Id}, Manufacture name: {manufacture.ManufactureName}");
                Console.WriteLine("______________________________________________________");
            }
            Console.WriteLine("Enter id for the manufacture you want to update: ");
            var manufactureId = int.Parse(Console.ReadLine()!);

            var manufactureEntity = _manufactureService.GetOneManufacture(x => x.Id == manufactureId);
            if (manufactureEntity != null)
            {
                Console.WriteLine($"Manufacture id: {manufactureEntity.Id}, RoleName: {manufactureEntity.ManufactureName}");

                if (!manufactureEntity.Products.Any())
                {
                    Console.WriteLine("No Products is currently in this manufacturer");
                }
                else
                {
                    Console.WriteLine("\n Products associated with manufacturers:");
                    foreach (var product in manufactureEntity.Products)
                    {
                        Console.WriteLine($"Product id: {product.Id}. Product name: {product.ProductName}, price: {product.ProductPrice}");
                        Console.WriteLine("______________________________________________________");
                    }
                    Console.WriteLine("Enter product´s id you want to update manufacturer for: ");
                    var productId = int.Parse(Console.ReadLine()!);

                    var productEntity = _productService.GetOneProduct(x => x.Id == productId);
                    if (productEntity != null)
                    {
                        Console.WriteLine("Selected product:");
                        Console.WriteLine($"Product id: {productEntity.Id}. Product name: {productEntity.ProductName}, price: {productEntity.ProductPrice}");

                        Console.WriteLine("Enter manufacture name: ");
                        var manufactureName = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(manufactureName))
                        {
                            Console.WriteLine("You have to enter a role name");
                            PressToContinue();
                            return;
                        }

                        var existingManufacture = _manufactureService.GetOneManufacture(x => x.ManufactureName == manufactureName);
                        if (existingManufacture == null)
                        {
                            existingManufacture = new Manufacture { ManufactureName = manufactureName };
                        }

                        productEntity.Manufacture = existingManufacture;
                        _productService.UpdateProduct(productEntity);
                        Console.WriteLine("Updated successfully!");
                    }
                }
            }

            PressToContinue();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
        }
    }

    public void DeleteManufactureMenu()
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
            Console.WriteLine("No product was found");
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

            Console.WriteLine("9. Back to main menu");

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

                case "9":
                    StartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again");
                    PressToContinue();
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

    public void GetOneOrderMenu()
    {
        try
        {
            ClearAndTitle("Show One Order");

            var allOrderRows = _orderRowService.GetOrdersWithSameId();

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
            var orderId = int.Parse(Console.ReadLine()!);

            if (allOrders.Any())
            {
                var orderRows = allOrders.Where(o => o.OrderId == orderId).ToList();
                if (orderRows.Any())
                {
                    Console.WriteLine($"Products in Order id {orderId}:");
                    foreach (var orderRow in orderRows)
                    {
                        Console.WriteLine($"Order Row id: {orderRow.Id}, ProductName: {orderRow.Product.ProductName}, Quantity: {orderRow.Quantity}");
                    }

                    Console.WriteLine("Enter Order Row id for the product you want to update: ");
                    var orderRowId = int.Parse(Console.ReadLine()!);

                    var getOneOrderRow = orderRows.FirstOrDefault(o => o.Id == orderRowId);
                    if (getOneOrderRow != null)
                    {
                        Console.WriteLine($"Selected orderrow: ");
                        Console.WriteLine($"Order Row id: {getOneOrderRow.Id}, ProductName: {getOneOrderRow.Product.ProductName}, Quantity: {getOneOrderRow.Quantity}");

                        Console.WriteLine("Do you want to change product? (y/n)");
                        var changeProduct = Console.ReadLine()!.ToLower();
                        if (changeProduct.Equals("y"))
                        {
                            var products = _productService.GetAllProducts();
                            foreach (var product in products)
                            {
                                Console.WriteLine($"product id: {product.Id}. Product name: {product.ProductName}");
                            }
                            Console.Write("Enter product id to change current product: ");
                            var productId = int.Parse(Console.ReadLine()!);

                            var productEntity = _productService.GetOneProduct(x => x.Id == productId);
                            if (productEntity != null)
                            {
                                // Uppdatera den befintliga orderpostens produkt med den nya produkten
                                getOneOrderRow.ProductId = productEntity.Id;

                                Console.WriteLine($"New orderrow productname: {productEntity.ProductName}");
                            }
                        }
                        Console.WriteLine("\nLeave empty if you dont want to change\n");
                        Console.Write("Enter new quantity: ");
                        var newQuantityInput = int.Parse(Console.ReadLine()!);
                        if (newQuantityInput > 0)
                        {
                            getOneOrderRow.Quantity = newQuantityInput;
                        }

                        _orderRowService.UpdateOrderRow(getOneOrderRow);
                        Console.WriteLine("Order row updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Order Row id.");
                    }
                }
                else
                {
                    Console.WriteLine($"No products found for Order id {orderId}.");
                }
            }
            PressToContinue();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
        }
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

            Console.WriteLine("9. Back to main menu");
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

                case "9":
                    StartMenu();
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again");
                    PressToContinue();
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
                    Console.WriteLine("Review was created successfully");
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


    //<<<<<<<<<<<DESCRIPTION>>>>>>>>>>>>>>>>>>
    public void DescriptionMenu()
    {
        //create and delete is being bound to product
        try
        {
            ClearAndTitle("Description Menu");
            Console.WriteLine("1. Get Description For Specific Product");
            Console.WriteLine("2. Get All Descriptions");
            Console.WriteLine("3. Update Description");
            Console.WriteLine("9. Back to main menu");
            Console.Write("Enter option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    GetDescriptionForOneProductMenu();
                    break;

                case "2":
                    GetAllDescriptionsMenu();
                    break;

                case "3":
                    UpdateDescriptionMenu();
                    break;

                case "9":
                    StartMenu(); 
                    break;

                default:
                    Console.WriteLine("Invalid choice, try again");
                    PressToContinue();
                    break;
            }

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetDescriptionForOneProductMenu()
    {
        try
        {
            ClearAndTitle("Get Description For Specific Product");

            DisplayProductIdAndName();

            Console.Write("Enter the product ID to see the description: ");
            var productId = int.Parse(Console.ReadLine()!);

            var description = _descriptionService.GetOneDescription(d => d.Products.Any(p => p.Id == productId));

            if (description != null)
            {
                Console.WriteLine($"Description for product ID {productId}:");
                Console.WriteLine($"Ingress: {description.Ingress}");
                Console.WriteLine($"Description: {description.DescriptionText}");
            }
            else
            {
                Console.WriteLine($"No description found.");
            }

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetAllDescriptionsMenu()
    {
        try
        {
            ClearAndTitle("Get All Descriptions");

            var allDescriptions = _descriptionService.GetAllDescriptions();

            foreach (var description in allDescriptions)
            {
                // hämta produktnamnet från den associerade produkten för beskrivningen
                var productName = description.Products.FirstOrDefault(x => x.DescriptionId == description.Id)!.ProductName;

                Console.WriteLine($"Description for: {productName}");
                Console.WriteLine($"Ingress: {description.Ingress}");
                Console.WriteLine($"Description text: {description.DescriptionText}");
                Console.WriteLine("____________________________________"); 
            }

            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void UpdateDescriptionMenu()
    {
        try
        {
            ClearAndTitle("Update Description");

            DisplayProductIdAndName();
            Console.WriteLine("Enter a product to update ingress and description text");
            var productId = int.Parse(Console.ReadLine()!);

            var productEntity = _productService.GetOneProduct(x => x.Id == productId);
            if( productEntity != null )
            {
                Console.WriteLine("Enter ingress: ");
                var ingress = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(ingress))
                {
                    Console.WriteLine("Can´t be empty, try again!");
                    PressToContinue();
                    return;
                }

                Console.WriteLine("\nEnter descritpion: ");
                var descriptionText = Console.ReadLine();

                var existingDescriptionEntity = _descriptionService.GetOneDescription(x => x.Products.Any(p => p.Id == productId));


                var newDescriptionEntity = new Description
                {
                    Id = existingDescriptionEntity.Id,
                    Products = existingDescriptionEntity.Products,
                    Ingress = ingress,
                    DescriptionText = descriptionText
                };

                var updatedDescriptionEntity = _descriptionService.UpdateDescription(newDescriptionEntity);
                if(updatedDescriptionEntity != null)
                {
                    Console.WriteLine("Description updated successfully!");
                }
                else
                {
                    Console.WriteLine("Failed to update description");
                }
            }
            else
            {
                Console.WriteLine("Product was not found");
            }


            PressToContinue();

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
}
