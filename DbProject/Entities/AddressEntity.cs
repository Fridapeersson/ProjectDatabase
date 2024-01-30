using System.ComponentModel.DataAnnotations;

namespace DbProject.Entities;

public class AddressEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Street { get; set; } = null!;
    [Required]
    public string PostalCode { get; set; } = null!;

    [Required]
    public string City { get; set; } = null!;


    public virtual ICollection<CustomerEntity> Customers { get; set; } = new HashSet<CustomerEntity>();
}
