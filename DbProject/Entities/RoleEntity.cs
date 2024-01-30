using System.ComponentModel.DataAnnotations;

namespace DbProject.Entities;

public class RoleEntity
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string RoleName { get; set; } = null!;


    public virtual ICollection<CustomerEntity> Customers { get; set; } = new HashSet<CustomerEntity>();
}
