using System;
using System.Collections.Generic;
using ERP;
using Microsoft.EntityFrameworkCore;

namespace MinimalAPIERP.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Raincheck> Rainchecks { get; set; }

    public virtual DbSet<Store> Stores { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\mssqllocaldb;Initial Catalog=PartsUnlimitedWebsite;Integrated Security=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PK_dbo.CartItems");

            entity.HasOne(d => d.Product).WithMany(p => p.CartItems).HasConstraintName("FK_dbo.CartItems_dbo.Products_ProductId");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK_dbo.Categories");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK_dbo.Orders");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK_dbo.OrderDetails");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails).HasConstraintName("FK_dbo.OrderDetails_dbo.Orders_OrderId");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails).HasConstraintName("FK_dbo.OrderDetails_dbo.Products_ProductId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK_dbo.Products");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).HasConstraintName("FK_dbo.Products_dbo.Categories_CategoryId");
        });

        modelBuilder.Entity<Raincheck>(entity =>
        {
            entity.HasKey(e => e.RaincheckId).HasName("PK_dbo.Rainchecks");

            entity.HasOne(d => d.Product).WithMany(p => p.Rainchecks).HasConstraintName("FK_dbo.Rainchecks_dbo.Products_ProductId");

            entity.HasOne(d => d.Store).WithMany(p => p.Rainchecks).HasConstraintName("FK_dbo.Rainchecks_dbo.Stores_StoreId");
        });

        modelBuilder.Entity<Store>(entity =>
        {
            entity.HasKey(e => e.StoreId).HasName("PK_dbo.Stores");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
