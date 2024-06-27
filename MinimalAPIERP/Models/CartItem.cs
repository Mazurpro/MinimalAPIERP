using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP;

[Index("ProductId", Name = "IX_ProductId")]
public partial class CartItem
{
    [Key]
    public int CartItemId { get; set; }

    public string CartId { get; set; } = null!;

    public int ProductId { get; set; }

    public int Count { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateCreated { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("CartItems")]
    public virtual Product Product { get; set; } = null!;
}
