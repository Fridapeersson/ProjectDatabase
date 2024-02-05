using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Entities;

[Table("Description")]
[Index("Ingress", Name = "UQ__Descript__E92241C4808859CD", IsUnique = true)]
public partial class Description
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Ingress { get; set; } = null!;

    public string? DescriptionText { get; set; }

    [InverseProperty("Description")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
