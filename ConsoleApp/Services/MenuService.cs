using DbProject.Dtos;
using DbProject.Entities;
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

    public MenuService(CustomerService customerService, RoleService roleService, AddressService addressService)
    {
        _customerService = customerService;
        _roleService = roleService;
        _addressService = addressService;
    }

    public void StartMenu()
    {
        while(true)
        {
            Console.Clear();
            Console.WriteLine("Database Project\n\n");

            Console.WriteLine("1. Create Customer");
            Console.WriteLine("2. Get One Customer");
            Console.WriteLine("3. Get All Customer");
            Console.WriteLine("4. Update Customer");
            Console.WriteLine("5. Delete Customer");
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

                default:
                    Console.WriteLine("Invalid option");
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
        catch(Exception ex) { Debug.WriteLine("ERROR :: " + ex); }
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

            DisplayIdAndName();

            Console.Write($"Enter id to se all details: ");
            var customerId = Console.ReadLine();

            if(int.TryParse(customerId, out int inputId) )
            {
                var selectedCustomer = _customerService.GetOneCustomer(x => x.Id ==  inputId);
                if(selectedCustomer != null)
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

            DisplayIdAndName();

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

                    if(customerEntity != null)
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

            DisplayIdAndName();
            Console.Write("Enter id to delete customer: ");
            var input = int.Parse(Console.ReadLine()!);
            var customerEntity = _customerService.GetOneCustomer(x => x.Id == input);



            //var result = _customerService.DeleteCustomer(x => x.Id == input);

            if (customerEntity != null)
            {
                Console.WriteLine($"Are you sure you want to delete {customerEntity.FirstName} {customerEntity.LastName}? (y/n)");
                var answer = Console.ReadLine()!.ToLower();
                if(answer.Equals("y")) 
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


    private void DisplayIdAndName()
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
}


