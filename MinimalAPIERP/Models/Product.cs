using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP;

[Index("CategoryId", Name = "IX_CategoryId")]
public partial class Product
{
    [Key]
    public int ProductId { get; set; }

    public string SkuNumber { get; set; } = null!;

    public int CategoryId { get; set; }

    public int RecommendationId { get; set; }

    [StringLength(160)]
    public string Title { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal SalePrice { get; set; }

    [StringLength(1024)]
    public string? ProductArtUrl { get; set; }

    public string Description { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime Created { get; set; }

    public string ProductDetails { get; set; } = null!;

    public int Inventory { get; set; }

    public int LeadTime { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    [InverseProperty("Product")]
    public virtual ICollection<Raincheck> Rainchecks { get; set; } = new List<Raincheck>();
}