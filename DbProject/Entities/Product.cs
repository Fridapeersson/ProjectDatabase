using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Entities;

[Table("Product")]
[Index("ProductName", Name = "UQ__Product__DD5A978AACCEF8C1", IsUnique = true)]
public partial class Product
{
    [Key]
    public int Id { get; set; }

    [StringLength(50)]
    public string ProductName { get; set; } = null!;

    [Column(TypeName = "money")]
    public decimal ProductPrice { get; set; }

    public int DescriptionId { get; set; }

    public int CategoryId { get; set; }

    public int ManufactureId { get; set; }

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [ForeignKey("DescriptionId")]
    [InverseProperty("Products")]
    public virtual Description Description { get; set; } = null!;

    [ForeignKey("ManufactureId")]
    [InverseProperty("Products")]
    public virtual Manufacture Manufacture { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderRow> OrderRows { get; set; } = new List<OrderRow>();

    [InverseProperty("Product")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
