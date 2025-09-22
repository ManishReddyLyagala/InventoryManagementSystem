using InventoryManagement_Backend.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManagement.Models
{
    public enum SupplierCategoryType
    {
        New,
        Preferred,
        Backup,
        HighRisk
    }

    public class SupplierCategory
    {
        [Key]
        public int SupplierCategoryId { get; set; }

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier Supplier { get; set; }

        public SupplierCategoryType Category { get; set; } = SupplierCategoryType.New;

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
