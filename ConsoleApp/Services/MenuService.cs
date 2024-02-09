using DbProject.Dtos;
using DbProject.Entities;
using DbProject.Services;
using System.Data;
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
            Console.WriteLine("9. Description Menu");


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

            Console.WriteLine("\n9. Back to main menu");
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

            var firstName = GetValidStringUserInput("First name: ");
            var lastName = GetValidStringUserInput("Last name: ");
            var email = GetValidStringUserInput("Email: ");
            var roleName = GetValidStringUserInput("Role");
            var street = GetValidStringUserInput("Streetname: ");
            var postalCode = GetValidStringUserInput("Postal code: ");
            var city = GetValidStringUserInput("City: ");

            var existingAddress = _addressService.GetOneAddress(x => x.Street.ToLower() == street && x.PostalCode.ToLower() == postalCode && x.City.ToLower() == city);

            if (existingAddress != null)
            {
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
                // Creating new customer and address
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
            ClearAndTitle("Get One Customer");

            DisplayCustomerIdAndName();

            var customerId = GetValidIdUserInput("Enter id to se all details: ");
            var selectedCustomer = _customerService.GetOneCustomer(x => x.Id == customerId);
            
            if (selectedCustomer != null)
            {
                Console.WriteLine("\nSelected customer:");
                Console.WriteLine($"{selectedCustomer.Id}. Firstname: {selectedCustomer.FirstName}, Lastname: {selectedCustomer.LastName}, Email: {selectedCustomer.Email}, Street: {selectedCustomer.Address.Street}, PostalCode: {selectedCustomer.Address.PostalCode}, City: {selectedCustomer.Address.City}, Role: {selectedCustomer.Role.RoleName}");
            }
            else
            {
                Console.WriteLine("No customer found");
            }

            PressToContinue();

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void UpdateCustomerMenu()
    {
        try
        {
            ClearAndTitle("Update Customer");

            DisplayCustomerIdAndName();

            //Console.Write($"Enter id to update customer: ");
            //var customerId = Console.ReadLine();
            var customerId = GetValidIdUserInput("Enter id to update customer: ");

            //if (int.TryParse(customerId, out int inputId))
            //{
                var customerEntity = _customerService.GetOneCustomer(x => x.Id == customerId);
                if (customerEntity != null)
                {
                    Console.WriteLine("\nSelected customer:");
                    Console.WriteLine($"{customerEntity.Id}. Firstname: {customerEntity.FirstName}, Lastname: {customerEntity.LastName}, Email: {customerEntity.Email}");

                var newEmail = GetValidStringUserInput("\nEnter new email: ");
                    //Console.Write("\nEnter new email: ");
                    //var newEmailInput = Console.ReadLine()!.ToLower();

                    if (!string.IsNullOrWhiteSpace(newEmail))
                    {
                        var existingEmail = _customerService.GetOneCustomer(x => x.Email == newEmail);
                        if (existingEmail == null)
                        {
                            customerEntity.Email = newEmail;
                        }
                        else
                        {
                            Console.WriteLine("Email already exists. Try again");
                            PressToContinue();
                            return;
                        }
                    }
                //else
                //{
                //    Console.WriteLine("You need to enter an email");
                //    PressToContinue();
                //    return;
                //}

                //Console.Write("\nEnter new first name: ");
                //var newFirstNameInput = Console.ReadLine()?.ToLower();
                var newFirstName = GetValidStringUserInput("\nEnter new first name: ");
                if (!string.IsNullOrWhiteSpace(newFirstName))
                {
                    customerEntity.FirstName = newFirstName;
                }
                //Console.Write("\nEnter new lastname: ");
                //    var newLastNameInput = Console.ReadLine()?.ToLower();

                var newLastName = GetValidStringUserInput("\nEnter new lastname: ");
                if (!string.IsNullOrWhiteSpace(newLastName))
                {
                    customerEntity.LastName = newLastName;
                }

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
            //}
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
            //Console.Write("Enter id to delete customer: ");
            //var input = int.Parse(Console.ReadLine()!);
            var input = GetValidIdUserInput("Enter id to delete customer: ");
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
            while(true) 
            {
                ClearAndTitle("Address Menu");


                Console.WriteLine("1. Create Address");
                Console.WriteLine("2. Get One Address");
                Console.WriteLine("3. Get All Addresses");
                Console.WriteLine("4. Update Address");
                Console.WriteLine("5. Delete Address");

                Console.WriteLine("\n9. Back to main menu");
                Console.Write("Enter your option: ");
                var option = Console.ReadLine();

                switch (option)
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
            

        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
    
    public void CreateAddressMenu()
    {
        try
        {
            ClearAndTitle("CreateAddress");

            var streetName = GetValidStringUserInput("Enter street name: ");
            var postalCode = GetValidStringUserInput("Enter postal code: ");
            var city = GetValidStringUserInput("Enter city: ");

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
            var addressId = GetValidIdUserInput("Enter id to view details: ");

            var addressEntity = _addressService.GetOneAddress(x => x.Id ==  addressId);

            if(addressEntity != null)
            {
                Console.WriteLine($"Id: {addressEntity.Id}. Street name: {addressEntity.Street} Postal code: {addressEntity.PostalCode} City: {addressEntity.City}");
            }
            else
            {
                Console.WriteLine("No address found");
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

            var addressId = GetValidIdUserInput("\nEnter id for the address you want to update: ");

            var addressEntity = _addressService.GetOneAddress(x => x.Id == addressId);
            if (addressEntity == null)
            {
                Console.WriteLine("No address found, try again");
                PressToContinue();
                return;
            }
            if (AddressHasCustomers(addressId))
            {
                //gets the customers associated with specified address and display
                var customersOnAddress = _customerService.GetAllCustomers().Where(x => x.Address.Id == addressId);

                Console.WriteLine("Customers on specific address: ");
                foreach (var customer in customersOnAddress)
                {
                    Console.WriteLine($"Id: {customer.Id}. Firstname: {customer.FirstName}, Lastname: {customer.LastName}, Email: {customer.Email}");
                }

                var customerId = GetValidIdUserInput("Enter customer´s id you want to update the address for: ");

                var customerEntity = _customerService.GetOneCustomer(x => x.Id == customerId && x.Address.Id == addressId);

                if (customerEntity != null)
                {
                    Console.WriteLine("\nSelected customer:");
                    Console.WriteLine($"{customerEntity.Id}. Firstname: {customerEntity.FirstName}, Lastname: {customerEntity.LastName}, Email: {customerEntity.Email}");

                    Console.WriteLine("\nSelected address:");
                    Console.WriteLine($"Id: {addressEntity.Id}. Street name: {addressEntity.Street}, Postalcode: {addressEntity.PostalCode}, City: {addressEntity.City})");

                    var newStreetInput = GetValidStringUserInput("\nEnter new street name: ");
                    var newPostalCodeInput = GetValidStringUserInput("\nEnter new postal code: ");
                    var newCityInput = GetValidStringUserInput("\nEnter new city: ");

                    // kollar om den nya adressen finns i databasen 
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

                    Console.WriteLine("Customer updated successfully");
                }
                else
                {
                    Console.WriteLine("Customer not found or not associated with the selected address.");
                }
            }
            else
            {
                Console.WriteLine("\nSelected address:");
                Console.WriteLine($"Id: {addressEntity.Id}. Street name: {addressEntity.Street}, Postalcode: {addressEntity.PostalCode}, City: {addressEntity.City})");

                var newStreetInput = GetValidStringUserInput("\nEnter new street name: ");
                var newPostalCodeInput = GetValidStringUserInput("\nEnter new postal code: ");
                var newCityInput = GetValidStringUserInput("\nEnter new city: ");

                // Check if the new address already exisits 
                var existingAddress = _addressService.GetOneAddress(x => x.Street == newStreetInput && x.PostalCode == newPostalCodeInput && x.City == newCityInput);
                //if the new address doesn't exist, creates a new addressentity
                if (existingAddress == null)
                {
                    addressEntity.Street = newStreetInput;
                    addressEntity.PostalCode = newPostalCodeInput;
                    addressEntity.City = newCityInput;

                    _addressService.UpdateAddress(addressEntity);
                    Console.WriteLine("Updated successfully!");
                }
                else
                {
                    Console.WriteLine("Address already exists");
                }
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

                var inputId = GetValidIdUserInput("Enter id to delete address: ");

                if(!AddressHasCustomers(inputId))
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

    /// <summary>
    ///     Checks if there is customer associated with the address based on specified addressId
    /// </summary>
    /// <param name="addressId">The id of the addressentity to check</param>
    /// <returns>True if there is customers associated with address entity, else false</returns>
    public bool AddressHasCustomers(int addressId)
    {
        try
        {
            //gets customers associated with the address
            var customerInAddress = _customerService.GetAllCustomers().Where(x => x.AddressId == addressId);
            if (customerInAddress.Any())
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
    }

    //<<<<<<<<<<<<<<<<<<<<<<<ROLE>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void RoleMenu()
    {
        try
        {
            while(true)
            {
                ClearAndTitle("Role Menu");

                Console.WriteLine("1. Create Role");
                Console.WriteLine("2. Get One Role");
                Console.WriteLine("3. Get All Roles");
                Console.WriteLine("4. Update Role");
                Console.WriteLine("5. Delete Role");

                Console.WriteLine("\n9. Back to main menu");

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
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateRoleMenu()
    {
        try
        {
            ClearAndTitle("Create Role");

            var roleName = GetValidStringUserInput("Enter role name: ");

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
            var roleId = GetValidIdUserInput("Enter role name: ");

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

            var roleId = GetValidIdUserInput("Enter id for the role you want to update: ");

            var roleEntity = _roleService.GetOneRole(x => x.Id == roleId);
            if (roleEntity != null)
            {
                Console.WriteLine($"RoleId: {roleEntity.Id}. Role name: {roleEntity.RoleName} ");

                if (RoleHasCustomers(roleId))
                {
                    Console.WriteLine("\nCustomers connected to role:");
                    foreach (var customer in roleEntity.Customers)
                    {
                        Console.WriteLine($"customer id: {customer.Id}, Customer name: {customer.FirstName} {customer.LastName} Email: {customer.Email}");
                        Console.WriteLine("______________________________________________________");
                    }

                    var customerId = GetValidIdUserInput("Enter customer´s id to update role: ");


                    var customerEntity = _customerService.GetOneCustomer(x => x.Id == customerId);
                    if (customerEntity != null)
                    {
                        Console.WriteLine("\nSelected customer:");
                        Console.WriteLine($"Customer id: {customerEntity.Id}, customer name: {customerEntity.FirstName} {customerEntity.LastName}, Email: {customerEntity.Email}");

                        string roleName = GetValidStringUserInput("\nEnter new role name: ");
                        if(string.IsNullOrEmpty(roleName) || string.IsNullOrWhiteSpace(roleName))
                        {
                            Console.WriteLine("You have to enter a role name");
                            PressToContinue();
                            return;
                        }
                        //Checks if role already exists in databse, if not create a new role entity
                        var existingRole = _roleService.GetOneRole(x => x.RoleName == roleName);
                        if(existingRole == null)
                        {
                            existingRole = new RoleEntity
                            {
                                RoleName = roleName!
                            };
                        }
                        customerEntity.Role = existingRole;
                        _customerService.UpdateCustomer(customerEntity);
                        Console.WriteLine("Updated successfully!");
                    }
                    else
                    {
                        Console.WriteLine("No customer found, try again");
                    }
                }
                else
                {
                    Console.WriteLine("No customers is currently in this role");
                    var roleName = GetValidStringUserInput("\nChange role name: ");
                    if(!string.IsNullOrWhiteSpace(roleName))
                    {
                        roleEntity.RoleName = roleName;
                        _roleService.UpdateRole(roleEntity);
                        Console.WriteLine("\nRole updated successfully");
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong, try again!");
                    }
                }
            }
            else
            {
                Console.WriteLine("No role found, try again");
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

                var inputId = GetValidIdUserInput("Enter id to delete role: ");

                if (!RoleHasCustomers(inputId))
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
                    Console.WriteLine("Role is not empty, make sure to remove all customers before deleting role");
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

    /// <summary>
    ///     Checks if there is customer associated with the role based on specified roleId
    /// </summary>
    /// <param name="roleId">The id of the roleentity to check</param>
    /// <returns>True if there is customers associated with role entity, else false</returns>
    public bool RoleHasCustomers(int roleId)
    {
        try
        {
            //gets customers associated with specific role
            var customerInAddress = _customerService.GetAllCustomers().Where(x => x.RoleId == roleId);
            if (customerInAddress.Any())
            {
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
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

    //<<<<<<<<<<<<<<<<<<<<<<PRODUCT CATALOG>>>>>>>>>>>>>>>>>>>>>>>>

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

            var productName = GetValidStringUserInput("Enter product name: ");
            //Check if product already is in database
            var existingProduct = _productService.GetOneProduct(x => x.ProductName == productName);
            if (existingProduct != null)
            {
                Console.WriteLine("Product already exists.");
                PressToContinue();
                return;
            }

            decimal productPrice;
            while (true)
            {
                Console.Write("Enter product price: ");
                string userInput = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("No input provided. Please try again.");
                }
                else if (!decimal.TryParse(userInput, out productPrice))
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal number.");
                }
                else if (productPrice <= 0)
                {
                    Console.WriteLine("Price must be greater than zero. Please try again.");
                }
                else
                {
                    break;
                }
            }

            var categoryName = GetValidStringUserInput("Enter category name: ");
            var manufacureName = GetValidStringUserInput("Enter manufacture name: ");
            var ingress = GetValidStringUserInput("Enter description ingress: ");

            Console.Write("Enter description (optional): ");
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
                Console.WriteLine($"Product: {productName} has been added");
                PressToContinue();
            }
            else
            {
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

        var productId = GetValidIdUserInput("Enter id to see details: ");
        
        var selectedProduct = _productService.GetOneProduct(x => x.Id == productId);
        if (selectedProduct != null)
        {
            Console.WriteLine($"Id: {selectedProduct.Id}, Productname: {selectedProduct.ProductName}, Price: {selectedProduct.ProductPrice}, Category: {selectedProduct.Category.CategoryName}, Manufacture: {selectedProduct.Manufacture.ManufactureName}, Description ingress: {selectedProduct.Description.Ingress}, Description text: {selectedProduct.Description.DescriptionText} ");
        }
        else
        {
            Console.WriteLine("No product was found!");
        }
        PressToContinue();
    }

    public void GetAllProductsMenu()
    {
        try
        {
            ClearAndTitle("All Products");

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
            ClearAndTitle("Update Product");

            DisplayProductIdAndName();

            var productId = GetValidIdUserInput("Enter id for the product you want to update: ");

            var productEntity = _productService.GetOneProduct(x => x.Id == productId);
            if (productEntity != null)
            {
                Console.WriteLine($"Selected product: \nproduct id: {productEntity.Id}. Product name: {productEntity.ProductName}, product price: {productEntity.ProductPrice}");

                var newProductName = GetValidStringUserInput("\nEnter new product name: ");

                decimal newProductPrice;
                while (true)
                {
                    Console.Write("Enter new product price: ");
                    string userInput = Console.ReadLine()!;

                    if (string.IsNullOrWhiteSpace(userInput))
                    {
                        Console.WriteLine("No input provided. Please try again.");
                    }
                    else if (!decimal.TryParse(userInput, out newProductPrice))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid decimal number.");
                    }
                    else if (newProductPrice <= 0)
                    {
                        Console.WriteLine("Price must be greater than zero. Please try again.");
                    }
                    else
                    {
                        break;
                    }
                }
                productEntity.ProductName = newProductName;
                productEntity.ProductPrice = newProductPrice;
                var result = _productService.UpdateProduct(productEntity);
                if(result != null)
                {
                    Console.WriteLine("Updated product successfully");
                }
                else
                {
                    Console.WriteLine("Something went wrong, try again");
                }
                PressToContinue();
            }
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void DeleteProductMenu()
    {
        try
        {
            ClearAndTitle("Delete Product");
            DisplayProductIdAndName();

            var productId = GetValidIdUserInput("Enter id to delete product: ");

            var productEntity = _productService.GetOneProduct(x => x.Id == productId);

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
                Console.WriteLine("Product not found");
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
            while(true)
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
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }
    public void CreateCategoryMenu()
    {
        try
        {
            ClearAndTitle("Create Category");

            var categoryName = GetValidStringUserInput("Enter category name: ");
            //checks if category already exisits
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

            var categoryId = GetValidIdUserInput("Enter id to see details: ");
            var selectedCategory = _categoryService.GetOneCategory(x => x.Id == categoryId);
            if (selectedCategory != null)
            {
                Console.WriteLine($"Id: {selectedCategory.Id}, Category name: {selectedCategory.CategoryName}");
                Console.WriteLine($"Products conneted to the category: ");
                foreach (var product in selectedCategory.Products)
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

            var categoryId = GetValidIdUserInput("Enter id for the category you want to update: ");

            var categoryEntity = _categoryService.GetOneCategory(x => x.Id == categoryId);
            if (categoryEntity != null)
            {
                Console.WriteLine($"Category id: {categoryEntity.Id}, Category name: {categoryEntity.CategoryName}");

                if (HasProducts(categoryId))
                {
                    Console.WriteLine($"\n Products associated with category:");
                    foreach (var product in categoryEntity.Products)
                    {
                        Console.WriteLine($"Product id: {product.Id}. Product name: {product.ProductName}, price: {product.ProductPrice}");
                        Console.WriteLine("______________________________________________________");
                    }

                    var productId = GetValidIdUserInput("Enter product´s id you to update category: ");

                    var productEntity = _productService.GetOneProduct(x => x.Id == productId);
                    if (productEntity != null)
                    {
                        Console.WriteLine("Selected product:");
                        Console.WriteLine($"Product id: {productEntity.Id}. Product name: {productEntity.ProductName}, price: {productEntity.ProductPrice}");

                        var categoryName = GetValidStringUserInput("\nEnter category name: ");
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
                else
                {
                    Console.WriteLine("\nSelected category:");
                    Console.WriteLine($"Id: {categoryEntity.Id}. Category name: {categoryEntity.CategoryName})");

                    var newCategoryName = GetValidStringUserInput("Enter new category name: ");

                    //kolla om den nya kategorin redan finns i database
                    var existingCategory = _categoryService.GetOneCategory(x => x.CategoryName == newCategoryName);
                    if (existingCategory == null)
                    {
                        categoryEntity.CategoryName = newCategoryName;

                        _categoryService.UpdateCategory(categoryEntity);
                        Console.WriteLine("Updated category successfully");
                    }
                    else
                    {
                        Console.WriteLine("Category already exists");
                    }
                }
            }
            else
            {
                Console.WriteLine("No categories found");
            }
            PressToContinue();
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
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

                var categoryId = GetValidIdUserInput("Enter id to delete category: ");

                if (!HasProducts(categoryId))
                {
                    var deleted = _categoryService.DeleteCategory(x => x.Id == categoryId);
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

    /// <summary>
    ///     Checks if there are any products associated wioth the specified category
    /// </summary>
    /// <param name="categoryId">the id of the category to check for associated products</param>
    /// <returns>True if there is associated products, else false</returns>
    public bool HasProducts(int categoryId)
    {
        try
        {
            var productsInCategory = _productService.GetAllProducts().Where(p => p.CategoryId == categoryId);
            return productsInCategory.Any();
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR :: " + ex.Message);
            return false;
        }
    }


    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<Manufacture>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void ManufactureMenu()
    {
        try
        {
            while (true)
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
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateManufactureMenu()
    {
        try
        {
            ClearAndTitle("Create Manufacturer");

            var manufactureName = GetValidStringUserInput("Enter manufacurer name: ");

            //Checks if manufacturer already exisits
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

            var manufacureId = GetValidIdUserInput("Enter id to se details: ");
            var selectedManufacture = _manufactureService.GetOneManufacture(x => x.Id == manufacureId);
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
            foreach(var manufacture in manufacturers )
            {
                Console.WriteLine($"Manufacture id: {manufacture.Id}. Manufacture name: {manufacture.ManufactureName}");
            }
            var manufactureId = GetValidIdUserInput("\nEnter id for the manufacture you want to update: ");

            var manufactureEntity = _manufactureService.GetOneManufacture(x => x.Id == manufactureId);
            if (manufactureEntity != null)
            {
                Console.WriteLine($"Manufacture id: {manufactureEntity.Id}. Manufacture name: {manufactureEntity.ManufactureName}");

                if (manufactureEntity.Products.Any())
                {
                    Console.WriteLine("Product associated with manufacure: ");
                    foreach (var product in manufactureEntity.Products)
                    {
                        Console.WriteLine($"Product id: {product.Id}. Product name: {product.ProductName}, price: {product.ProductPrice}");
                        Console.WriteLine("______________________________________________________");
                    }
                    var productId = GetValidIdUserInput("Enter product´s id you to update manufacture: ");

                    var productEntity = _productService.GetOneProduct(x => x.Id == productId);
                    if (productEntity != null)
                    {
                        Console.WriteLine("Selected product:");
                        Console.WriteLine($"Product id: {productEntity.Id}. Product name: {productEntity.ProductName}, price: {productEntity.ProductPrice}");

                        var manufactureName = GetValidStringUserInput("\nEnter manufacture name: ");

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
                else
                {
                    Console.WriteLine("\nSelected manufacturer: ");
                    Console.WriteLine($"Manufacture id: {manufactureEntity.Id}. Manufacture name: {manufactureEntity.ManufactureName}");

                    var newManufactureName = GetValidStringUserInput("Enter new manufacture name: ");

                    var existingManufacture = _manufactureService.GetOneManufacture(x => x.ManufactureName == newManufactureName);
                    if (existingManufacture == null)
                    {
                        manufactureEntity.ManufactureName = newManufactureName;
                        _manufactureService.UpdateManufacture(manufactureEntity);
                        Console.WriteLine("Updated manufacture successfully");
                    }
                    else
                    {
                        Console.WriteLine("Manufacture already exists");
                    }
                }
            }
            else
            {
                Console.WriteLine("No manufacture with that id was found");
            }
            PressToContinue();
        }
        catch(Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
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

                var manufactureId = GetValidIdUserInput("Enter id to delete manufacture: ");

                if (!_manufactureService.HasProducts(manufactureId))
                {
                    var deleted = _manufactureService.DeleteManufacture(x => x.Id == manufactureId);
                    if (deleted)
                    {
                        Console.WriteLine($"Manufacture has been deleted successfully");
                        PressToContinue();
                    }
                    else
                    {
                        Console.WriteLine("Manufacture id couldn't be found, try again");
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


    /// <summary>
    ///     Gets all products and display id and name
    /// </summary>
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


    //<<<<<<<<<<<<<<<<<<<<<<<<<ORDERS>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
    public void OrderMenu()
    {
        try
        {
            while (true)
            {
                ClearAndTitle("Order Menu");

                Console.WriteLine("1. Create Order");
                Console.WriteLine("2. Get One Order");
                Console.WriteLine("3. Get All Orders");
                Console.WriteLine("4. Update Order");
                Console.WriteLine("5. Delete Order");

                Console.WriteLine("\n9. Back to main menu");
                Console.Write("Enter option: ");

                var option = Console.ReadLine();

                switch (option)
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
        }     
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateOrderMenu()
    {
        try
        {
            ClearAndTitle("Create Order Menu");

            var orderRows = new HashSet<OrderRowDto>();
            // keeps adding products until user enters "order" to order products
            while (true)
            {
                DisplayProductIdAndName();

                Console.Write("\nEnter product id to add to orderrow. When done, enter 'order' to order products: ");
                var input = Console.ReadLine()!.ToLower();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("You have to enter an id, try again!");
                    PressToContinue();
                    return;
                }

                if (input.Equals("order"))
                {
                    break;
                }

                if (!int.TryParse(input, out var productId))
                {
                    Console.WriteLine("Invalid input, you have to enter a id(number). Try again");
                    PressToContinue();
                    continue;
                }

                var product = _productService.GetOneProduct(x => x.Id == productId);
                if (product != null)
                {
                    Console.Write("Enter quantity: ");
                    var quantityInput = Console.ReadLine();
                    if (!int.TryParse(quantityInput, out var quantity) || quantity <= 0)
                    {
                        Console.WriteLine("Invalid input, quantity must be larger than 0, try again!");
                        PressToContinue();
                        continue;
                    }

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
            else
            {
                Console.WriteLine("Something went wrong, try again");
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

            var orderId = GetValidIdUserInput("Enter order id: ");

            var order = _orderRowService.GetAllOrderRows().Where(x => x.OrderId == orderId);

            if (order.Any())
            {
                foreach(var orderRow in order)
                {
                    Console.WriteLine($"Id: {orderRow.Id}. Product: {orderRow.Product.ProductName}, Quantity: {orderRow.Quantity}, Date: {orderRow.Order.Orderdate}");
                }

                var orderRowId = GetValidIdUserInput("\nEnter id to se details for specific product: ");

                var specificOrderRow = _orderRowService.GetOneOrderRow(x => x.Id == orderRowId);

                if(specificOrderRow != null)
                {
                    Console.WriteLine($"\nSpecific order: ");
                    Console.WriteLine($"Id: {specificOrderRow.Id}. Quantity {specificOrderRow.Quantity}, Product: {specificOrderRow.Product.ProductName}, Price: {specificOrderRow.Product.ProductPrice} SEK, Order date: {specificOrderRow.Order.Orderdate} ");
                }
                else
                {
                    Console.WriteLine("Order row not found");
                }
            }
            else
            {
                Console.WriteLine("Order id was not found");
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
            
            int orderId = GetValidIdUserInput("\nEnter id for the order you want to update: ");

            if (allOrders.Any())
            {
                var orderRows = allOrders.Where(x => x.OrderId == orderId).ToList();
                if (orderRows.Any())
                {
                    Console.WriteLine($"Products in Order id {orderId}:");
                    foreach (var orderRow in orderRows)
                    {
                        Console.WriteLine($"Order Row id: {orderRow.Id}, ProductName: {orderRow.Product.ProductName}, Quantity: {orderRow.Quantity}");
                    }
                    int orderRowId = GetValidIdUserInput("\nEnter Order Row id for the product you want to update: ");

                    var getOneOrderRow = orderRows.FirstOrDefault(x => x.Id == orderRowId);
                    if (getOneOrderRow != null)
                    {
                        Console.WriteLine($"Selected orderrow: ");
                        Console.WriteLine($"Order Row id: {getOneOrderRow.Id}, ProductName: {getOneOrderRow.Product.ProductName}, Quantity: {getOneOrderRow.Quantity}");

                        Console.WriteLine("\nDo you want to change product? (y/n)");
                        var changeProduct = Console.ReadLine()!.ToLower();
                        if (changeProduct.Equals("y"))
                        {
                            var products = _productService.GetAllProducts();
                            foreach (var product in products)
                            {
                                Console.WriteLine($"product id: {product.Id}. Product name: {product.ProductName}");
                            }
                            int productId = GetValidIdUserInput("\nEnter product id to change curren product: ");

                            var productEntity = _productService.GetOneProduct(x => x.Id == productId);
                            if (productEntity != null)
                            {
                                getOneOrderRow.ProductId = productEntity.Id;

                                Console.WriteLine($"New orderrow productname: {productEntity.ProductName}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid id, try again");
                                PressToContinue();
                                return;
                            }
                        }
                        Console.Write("Enter new quantity: ");

                        int newQuantityInput;
                        string userInput = Console.ReadLine()!;
                        if(string.IsNullOrEmpty(userInput))
                        {
                            Console.WriteLine("No quantity provided, order row has not been updated, try again");
                        }
                        else if(!int.TryParse(userInput, out newQuantityInput))
                        {
                            Console.WriteLine("Invalid input. Order row was not updated, try again");
                        }
                        else if(newQuantityInput <= 0)
                        {
                            Console.WriteLine("Quantity has to be greater than 0");
                        }
                        else
                        {
                            getOneOrderRow.Quantity = newQuantityInput;
                            _orderRowService.UpdateOrderRow(getOneOrderRow);
                            Console.WriteLine("Order row updated successfully");
                        }
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
            var orderId = GetValidIdUserInput("Which order do you want to delete? ");

            var result = _orderRowService.DeleteOrderRow(x => x.OrderId == orderId);
            if (result)
            {
                Console.WriteLine($"Order with OrderId {orderId} has been deleted.");
            }
            else
            {
                Console.WriteLine($"No order found with OrderId {orderId}.");
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
            while(true)
            {
                ClearAndTitle("Review Menu");
                Console.WriteLine("1. Create Review");
                Console.WriteLine("2. Get Reviews For Specific Product");
                Console.WriteLine("3. Get All Reviews");
                Console.WriteLine("4. Update Review");
                Console.WriteLine("5. Delete Review");

                Console.WriteLine("\n9. Back to main menu");
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
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void CreateReview()
    {
        try
        {
            ClearAndTitle("Review Menu");

            DisplayProductIdAndName();

            var productId = GetValidIdUserInput("Enter id for product to review: ");

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
            else
            {
                Console.WriteLine("No product id found, try again");
                PressToContinue();
                return;
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

            var productId = GetValidIdUserInput("Enter id to see reviews: ");

            var reviews = _reviewService.GetAllReviews().Where(x => x.ProductId == productId);

            if(reviews.Any())
            {
                var reviewCount = reviews.Count();
                Console.WriteLine($"Reviews for product:\n");
                foreach (var review in reviews)
                {
                    Console.WriteLine($"For product: {review.Product.ProductName}");
                    Console.WriteLine($"Review: {review.ReviewText}");
                    Console.WriteLine($"Created: {review.ReviewDate}");
                    Console.WriteLine("______________________________________\n");
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
            var productId = GetValidIdUserInput("Enter product id to update review text: ");

            var reviewsForProduct = allReviews.Where(x => x.Product.Id == productId).ToList();
            if (reviewsForProduct.Any())
            {
                Console.WriteLine($"\nReviews for product id {productId}:");
                foreach (var review in reviewsForProduct)
                {
                    Console.WriteLine($"Review id: {review.Id}. ReviewText: {review.ReviewText}, Date of review: {review.ReviewDate}        (Product: {review.Product.ProductName})");
                }

                var reviewId = GetValidIdUserInput("Enter id for the review you want to update: ");
                var reviewToUpdate = reviewsForProduct.FirstOrDefault(x => x.Id == reviewId);
                if (reviewToUpdate != null)
                {
                    Console.WriteLine($"Selected review: ");
                    Console.WriteLine($"Review id: {reviewToUpdate.Id}. ReviewText: {reviewToUpdate.ReviewText}, Date of review: {reviewToUpdate.ReviewDate}        (Product: {reviewToUpdate.Product.ProductName})");

                    var newReviewText = GetValidStringUserInput("\nEnter new review text: ");
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
                    Console.WriteLine($"Review with id {reviewId} was not found.");
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

            var productId = GetValidIdUserInput("Enter productid to remove review: ");

            var reviewsForProduct = allReviews.Where(x => x.ProductId == productId).ToList();
            if (reviewsForProduct.Any())
            {
                Console.WriteLine($"\nReviews for product id {productId}:");
                foreach (var review in reviewsForProduct)
                {
                    Console.WriteLine($"Review id: {review.Id}. ReviewText: {review.ReviewText}, Date of review: {review.ReviewDate}        (Product: {review.Product.ProductName})");
                }

                var reviewId = GetValidIdUserInput("\nEnter id for the review you want to delete: ");

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
            while(true)
            {
                ClearAndTitle("Description Menu");
                Console.WriteLine("1. Get Description For Specific Product");
                Console.WriteLine("2. Get All Descriptions");
                Console.WriteLine("3. Update Description");
                
                Console.WriteLine("\n9. Back to main menu");
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
        }
        catch (Exception ex) { Console.WriteLine("ERROR :: " + ex.Message); }
    }

    public void GetDescriptionForOneProductMenu()
    {
        try
        {
            ClearAndTitle("Get Description For Specific Product");

            DisplayProductIdAndName();

            var productId = GetValidIdUserInput("Enter the product ID to see the description: ");

            var description = _descriptionService.GetOneDescription(x => x.Products.Any(x => x.Id == productId));

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
            var productId = GetValidIdUserInput("Enter a product to update ingress and description text: ");

            var productEntity = _productService.GetOneProduct(x => x.Id == productId);
            if( productEntity != null )
            {
                var ingress = GetValidStringUserInput("Enter ingress: ");


                if (string.IsNullOrWhiteSpace(ingress))
                {
                    Console.WriteLine("Can´t be empty, try again!");
                    PressToContinue();
                    return;
                }

                Console.WriteLine("\nEnter descritpion (optional): ");
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

    /// <summary>
    ///     Clears console and displays the title entered
    /// </summary>
    /// <param name="title">the title to display on the console window</param>
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

    /// <summary>
    ///     A method to get valid input from user
    /// </summary>
    /// <param name="promptText">Text to instruct the user what to enter</param>
    /// <returns>the valid input from the user</returns>
    public int GetValidIdUserInput(string promptText)
    {
        while (true)
        {
            Console.Write(promptText);
            var userInput = Console.ReadLine()!.ToLower();
            if(userInput.Equals("q"))
            {
                StartMenu();
            }

            if (int.TryParse(userInput, out var result))
            {
                return result;
            }
            else
            {
                Console.WriteLine("Invalid input. Try again or press 'q' to get to main menu.");
            }
        }
    }
    public string GetValidStringUserInput(string promptText)
    {
        while (true)
        {
            Console.Write(promptText);
            string userInput = Console.ReadLine()!.ToLower();

            if (string.IsNullOrWhiteSpace(userInput))
            {
                Console.WriteLine("Invalid input. Try again or enter 'q' to return to main menu");
            }
            else if (userInput.Equals("q"))
            {
                StartMenu();
                return null!; 
            }
            else
            {
                return userInput;
            }
        }
    }
}