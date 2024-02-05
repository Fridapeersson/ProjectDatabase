using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Entities;

[Table("Manufacture")]
[Index("ManufactureName", Name = "UQ__Manufact__00DD03CEC5DBE4D0", IsUnique = true)]
public partial class Manufacture
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ManufactureName { get; set; } = null!;

    [InverseProperty("Manufacture")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
