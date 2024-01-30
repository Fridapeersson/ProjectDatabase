namespace DbProject.Dtos;

public class CreateCustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
    public string RoleName { get; set; } = null!;

    public int AddressId { get; set; }
    public int RoleId { get; set; }
}
