using System;

namespace InventoryManagement.Dtos
{
    public class SupplierCategoryDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProductCategory { get; set; }
        public string Category { get; set; }
        public DateTime LastUpdated { get; set; }
    }

    public class UpdateSupplierCategoryDto
    {
        public int SupplierId { get; set; }
        public string Category { get; set; } // Preferred, Backup, etc.
    }
}
