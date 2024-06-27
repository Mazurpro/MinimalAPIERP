using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ERP;

public partial class Order
{
    [Key]
    public int OrderId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    public string Username { get; set; } = null!;

    [StringLength(160)]
    public string Name { get; set; } = null!;

    [StringLength(70)]
    public string Address { get; set; } = null!;

    [StringLength(40)]
    public string City { get; set; } = null!;

    [StringLength(40)]
    public string State { get; set; } = null!;

    [StringLength(10)]
    public string PostalCode { get; set; } = null!;

    [StringLength(40)]
    public string Country { get; set; } = null!;

    [StringLength(24)]
    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}