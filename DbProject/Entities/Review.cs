using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DbProject.Entities;

[Table("Review")]
public partial class Review
{
    [Key]
    public int Id { get; set; }

    public string ReviewText { get; set; } = null!;

    public DateTime ReviewDate { get; set; }

    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("Reviews")]
    public virtual Product Product { get; set; } = null!;
}
