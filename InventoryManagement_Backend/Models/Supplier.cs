using InventoryManagement.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Transactions;

namespace InventoryManagement_Backend.Models
{
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string MobileNumber { get; set; }
        public string EmailID { get; set; } = string.Empty;
        public string ProductCategory { get; set; } = string.Empty;

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<Transaction> PurchaseTransactions { get; set; } = new List<Transaction>();
        public SupplierCategory SupplierCategory { get; set; }
        public ICollection<SupplierOrder> SupplierOrders { get; set; }


    }
}
