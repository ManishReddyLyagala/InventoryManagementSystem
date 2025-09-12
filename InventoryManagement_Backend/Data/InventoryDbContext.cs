using InventoryManagement_Backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace InventoryManagement_Backend.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options)
        {
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Products -> Supplier (required)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Supplier)
                .WithMany(s => s.Products)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction -> Supplier (nullable)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Supplier)
                .WithMany(s => s.PurchaseTransactions)
                .HasForeignKey(t => t.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Transaction -> Customer (nullable)
            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.SalesTransactions)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // PurchaseOrder -> Transaction (1..M)
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Transaction)
                .WithMany(t => t.PurchaseOrders)
                .HasForeignKey(po => po.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            // PurchaseOrder -> Product (1..M)
            modelBuilder.Entity<PurchaseOrder>()
                .HasOne(po => po.Product)
                .WithMany(p => p.PurchaseOrders)
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            // CHECK constraint: Type must be 'P' or 'S'
            modelBuilder.Entity<Transaction>()
                .HasCheckConstraint("CK_Transactions_Type", "Type IN ('P','S')");

            // Business rule constraint:
            // If Type='P' then SupplierId IS NOT NULL AND CustomerId IS NULL
            // If Type='S' then CustomerId IS NOT NULL AND SupplierId IS NULL
            modelBuilder.Entity<Transaction>()
                .HasCheckConstraint("CK_Transactions_Business",
                    "((Type = 'P' AND SupplierId IS NOT NULL AND CustomerId IS NULL) OR (Type = 'S' AND CustomerId IS NOT NULL AND SupplierId IS NULL))");

            // Product.Quantity default 0 (already handled via model default) but also set default in DB
            modelBuilder.Entity<Product>()
                .Property(p => p.Quantity)
                .HasDefaultValue(0);

            // Indexes for common lookups
            modelBuilder.Entity<Product>().HasIndex(p => p.SupplierId);
            modelBuilder.Entity<Transaction>().HasIndex(t => new { t.Type, t.DateTime });
            modelBuilder.Entity<PurchaseOrder>().HasIndex(po => po.TransactionId);

        }
    }
}
