using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DbProject.Entities;

public class CustomerEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    public string Email { get; set; } = null!;


    [Required]
    [ForeignKey(nameof(RoleEntity))]
    public int RoleId { get; set; }

    [Required]
    [ForeignKey(nameof(AddressEntity))]
    public int AddressId { get; set; }


    public virtual AddressEntity Address { get; set; } = null!;
    public virtual RoleEntity Role { get; set; } = null!;
}
