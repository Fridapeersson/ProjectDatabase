using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Entities;

[Table("OrderRow")]
public partial class OrderRow
{
    [Key]
    public int Id { get; set; }

    public int Quantity { get; set; }

    public int ProductId { get; set; }

    public int OrderId { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("OrderRows")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("OrderRows")]
    public virtual Product Product { get; set; } = null!;
}
